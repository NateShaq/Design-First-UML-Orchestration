using System.Data;
using LcpUml3.Api.Data;
using LcpUml3.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmergencyCasesController(HospitalContext context) : ControllerBase
{
    private readonly HospitalContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmergencyCase>>> GetAll(CancellationToken cancellationToken)
        => await _context.EmergencyCases.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{recordId}")]
    public async Task<ActionResult<EmergencyCase>> GetById(string recordId, CancellationToken cancellationToken)
    {
        var entity = await _context.EmergencyCases.AsNoTracking().FirstOrDefaultAsync(e => e.RecordId == recordId, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<EmergencyCase>> Post(EmergencyCase emergencyCase, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.EmergencyCases.Add(emergencyCase);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { recordId = emergencyCase.RecordId }, emergencyCase);
    }

    [HttpPut("{recordId}")]
    public async Task<IActionResult> Put(string recordId, EmergencyCase emergencyCase, CancellationToken cancellationToken)
    {
        if (recordId != emergencyCase.RecordId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(emergencyCase).Property(e => e.RowVersion).OriginalValue = emergencyCase.RowVersion;
        _context.Entry(emergencyCase).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.EmergencyCases.AnyAsync(e => e.RecordId == recordId, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected; refresh and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{recordId}")]
    public async Task<IActionResult> Delete(string recordId, CancellationToken cancellationToken)
    {
        var entity = await _context.EmergencyCases.FindAsync(new object[] { recordId }, cancellationToken);
        if (entity is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.EmergencyCases.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
