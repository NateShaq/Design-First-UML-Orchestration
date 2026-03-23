using System.Data;
using IdentityPublicServices.Domain.Entities;
using IdentityPublicServices.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IdentityPublicServices.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuditTrailsController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuditTrailsController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AuditTrail>> Get(Guid id)
    {
        var audit = await _db.AuditTrails.AsNoTracking().FirstOrDefaultAsync(a => a.EventId == id);
        return audit is null ? NotFound() : Ok(audit);
    }

    [HttpPost]
    public async Task<ActionResult<AuditTrail>> Create(CreateAuditTrailRequest request)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var audit = new AuditTrail
        {
            EventId = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            TxId = request.TxId,
            IsolationLevel = request.IsolationLevel ?? "Serializable",
            CommitState = request.CommitState ?? "Pending",
            IdentityRegistryId = request.IdentityRegistryId,
            DataRetentionPolicyId = request.DataRetentionPolicyId
        };

        _db.AuditTrails.Add(audit);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();

        return CreatedAtAction(nameof(Get), new { id = audit.EventId }, audit);
    }
}

public record CreateAuditTrailRequest(
    Guid TxId,
    Guid IdentityRegistryId,
    Guid? DataRetentionPolicyId,
    string? IsolationLevel,
    string? CommitState);
