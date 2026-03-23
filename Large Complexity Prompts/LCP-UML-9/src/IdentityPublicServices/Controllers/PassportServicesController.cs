using System.Data;
using IdentityPublicServices.Domain.Entities;
using IdentityPublicServices.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityPublicServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PassportServicesController : ControllerBase
{
    private readonly AppDbContext _db;

    public PassportServicesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PassportService>> Get(Guid id)
    {
        var passportService = await _db.PassportServices
            .Include(p => p.Credential)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (passportService is null)
        {
            return NotFound();
        }

        return Ok(passportService);
    }

    [HttpPost]
    public async Task<ActionResult<PassportService>> Create(CreatePassportServiceRequest request)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var passport = new PassportService
        {
            Id = Guid.NewGuid(),
            PassportOffice = request.PassportOffice,
            IssuanceFee = request.IssuanceFee,
            NationalServiceId = request.NationalServiceId,
            CredentialId = request.CredentialId
        };

        _db.PassportServices.Add(passport);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        return CreatedAtAction(nameof(Get), new { id = passport.Id }, passport);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdatePassportServiceRequest request)
    {
        var passport = await _db.PassportServices.FirstOrDefaultAsync(p => p.Id == id);
        if (passport is null)
        {
            return NotFound();
        }

        // Concurrency token ensures ghost-write protection
        _db.Entry(passport).Property(p => p.RowVersion).OriginalValue = request.RowVersion;
        passport.PassportOffice = request.PassportOffice ?? passport.PassportOffice;
        passport.IssuanceFee = request.IssuanceFee ?? passport.IssuanceFee;
        passport.CredentialId = request.CredentialId ?? passport.CredentialId;

        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync();
            return Conflict("The passport service was updated by another transaction.");
        }

        return NoContent();
    }
}

public record CreatePassportServiceRequest(
    string PassportOffice,
    decimal IssuanceFee,
    Guid NationalServiceId,
    Guid? CredentialId);

public record UpdatePassportServiceRequest(
    string? PassportOffice,
    decimal? IssuanceFee,
    Guid? CredentialId,
    byte[] RowVersion);
