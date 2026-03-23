using System.Data;
using LcpUml3.Api.Data;
using LcpUml3.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OutpatientsController(HospitalContext context) : ControllerBase
{
    private readonly HospitalContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Outpatient>>> GetAll(CancellationToken cancellationToken)
        => await _context.Outpatients.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{recordId}")]
    public async Task<ActionResult<Outpatient>> GetById(string recordId, CancellationToken cancellationToken)
    {
        var entity = await _context.Outpatients.AsNoTracking().FirstOrDefaultAsync(e => e.RecordId == recordId, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<Outpatient>> Post(Outpatient outpatient, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Outpatients.Add(outpatient);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { recordId = outpatient.RecordId }, outpatient);
    }

    [HttpPut("{recordId}")]
    public async Task<IActionResult> Put(string recordId, Outpatient outpatient, CancellationToken cancellationToken)
    {
        if (recordId != outpatient.RecordId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(outpatient).Property(e => e.RowVersion).OriginalValue = outpatient.RowVersion;
        _context.Entry(outpatient).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.Outpatients.AnyAsync(e => e.RecordId == recordId, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected; refresh and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{recordId}")]
    public async Task<IActionResult> Delete(string recordId, CancellationToken cancellationToken)
    {
        var entity = await _context.Outpatients.FindAsync(new object[] { recordId }, cancellationToken);
        if (entity is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Outpatients.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
