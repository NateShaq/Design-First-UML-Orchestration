using IdentityPublicServices.Domain.Entities;
using IdentityPublicServices.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityPublicServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriversLicensesController : ControllerBase
{
    private readonly AppDbContext _db;

    public DriversLicensesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateDriversLicenseRequest request)
    {
        var license = await _db.DriversLicenses.FirstOrDefaultAsync(l => l.Id == id);
        if (license is null)
        {
            return NotFound();
        }

        _db.Entry(license).Property(l => l.RowVersion).OriginalValue = request.RowVersion;
        license.LicenseClass = request.LicenseClass ?? license.LicenseClass;
        license.IssuedState = request.IssuedState ?? license.IssuedState;
        license.CredentialId = request.CredentialId ?? license.CredentialId;

        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("The driver license record was updated by another transaction.");
        }

        return NoContent();
    }
}

public record UpdateDriversLicenseRequest(
    string? LicenseClass,
    string? IssuedState,
    Guid? CredentialId,
    byte[] RowVersion);
