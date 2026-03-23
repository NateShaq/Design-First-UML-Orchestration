using IdentityPublicServices.Domain.Entities;
using IdentityPublicServices.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityPublicServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VotingRegistriesController : ControllerBase
{
    private readonly AppDbContext _db;

    public VotingRegistriesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateVotingRegistryRequest request)
    {
        var voting = await _db.VotingRegistries.FirstOrDefaultAsync(v => v.Id == id);
        if (voting is null)
        {
            return NotFound();
        }

        _db.Entry(voting).Property(v => v.RowVersion).OriginalValue = request.RowVersion;
        voting.District = request.District ?? voting.District;
        voting.EligibilityRule = request.EligibilityRule ?? voting.EligibilityRule;
        voting.PublicRecordId = request.PublicRecordId ?? voting.PublicRecordId;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("The voting registry was updated by another transaction.");
        }

        return NoContent();
    }
}

public record UpdateVotingRegistryRequest(
    string? District,
    string? EligibilityRule,
    Guid? PublicRecordId,
    byte[] RowVersion);
