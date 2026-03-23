using System.Data;
using LcpUml3.Api.Data;
using LcpUml3.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InpatientsController(HospitalContext context) : ControllerBase
{
    private readonly HospitalContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Inpatient>>> GetAll(CancellationToken cancellationToken)
        => await _context.Inpatients.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{recordId}")]
    public async Task<ActionResult<Inpatient>> GetById(string recordId, CancellationToken cancellationToken)
    {
        var entity = await _context.Inpatients.AsNoTracking().FirstOrDefaultAsync(e => e.RecordId == recordId, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<Inpatient>> Post(Inpatient inpatient, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Inpatients.Add(inpatient);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { recordId = inpatient.RecordId }, inpatient);
    }

    [HttpPut("{recordId}")]
    public async Task<IActionResult> Put(string recordId, Inpatient inpatient, CancellationToken cancellationToken)
    {
        if (recordId != inpatient.RecordId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(inpatient).Property(e => e.RowVersion).OriginalValue = inpatient.RowVersion;
        _context.Entry(inpatient).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.Inpatients.AnyAsync(e => e.RecordId == recordId, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected; refresh and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{recordId}")]
    public async Task<IActionResult> Delete(string recordId, CancellationToken cancellationToken)
    {
        var entity = await _context.Inpatients.FindAsync(new object[] { recordId }, cancellationToken);
        if (entity is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Inpatients.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
