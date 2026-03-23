using System.Data;
using IdentityPublicServices.Domain.Entities;
using IdentityPublicServices.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityPublicServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityRegistriesController : ControllerBase
{
    private readonly AppDbContext _db;

    public IdentityRegistriesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<IdentityRegistry>>> Get()
    {
        var registries = await _db.IdentityRegistries
            .Include(r => r.Person)
            .Include(r => r.Jurisdiction)
            .Include(r => r.IdentityProvider)
            .AsNoTracking()
            .ToListAsync();
        return Ok(registries);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IdentityRegistry>> GetById(Guid id)
    {
        var registry = await _db.IdentityRegistries
            .Include(r => r.Person)
            .Include(r => r.Jurisdiction)
            .Include(r => r.IdentityProvider)
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.RegistryId == id);

        if (registry is null)
        {
            return NotFound();
        }

        return Ok(registry);
    }

    [HttpPost]
    public async Task<ActionResult<IdentityRegistry>> Create(CreateIdentityRegistryRequest request)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var person = await ResolvePersonAsync(request.Person);
        var registry = new IdentityRegistry
        {
            RegistryId = Guid.NewGuid(),
            Status = request.Status ?? "Active",
            Person = person,
            JurisdictionId = request.JurisdictionId,
            IdentityProviderId = request.IdentityProviderId
        };

        _db.IdentityRegistries.Add(registry);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        return CreatedAtAction(nameof(GetById), new { id = registry.RegistryId }, registry);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UpdateIdentityRegistryRequest request)
    {
        var registry = await _db.IdentityRegistries.FirstOrDefaultAsync(r => r.RegistryId == id);
        if (registry is null)
        {
            return NotFound();
        }

        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        registry.Status = request.Status ?? registry.Status;
        registry.IdentityProviderId = request.IdentityProviderId ?? registry.IdentityProviderId;
        registry.JurisdictionId = request.JurisdictionId ?? registry.JurisdictionId;

        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        return NoContent();
    }

    private async Task<Person> ResolvePersonAsync(PersonDto dto)
    {
        if (dto.PersonId.HasValue)
        {
            var existing = await _db.People.FindAsync(dto.PersonId.Value);
            if (existing is not null)
            {
                existing.FullName = dto.FullName ?? existing.FullName;
                return existing;
            }
        }

        return new Person
        {
            PersonId = dto.PersonId ?? Guid.NewGuid(),
            FullName = dto.FullName ?? string.Empty
        };
    }
}

public record PersonDto(Guid? PersonId, string? FullName);

public record CreateIdentityRegistryRequest(
    string? Status,
    PersonDto Person,
    Guid? JurisdictionId,
    Guid? IdentityProviderId);

public record UpdateIdentityRegistryRequest(
    string? Status,
    Guid? JurisdictionId,
    Guid? IdentityProviderId);
