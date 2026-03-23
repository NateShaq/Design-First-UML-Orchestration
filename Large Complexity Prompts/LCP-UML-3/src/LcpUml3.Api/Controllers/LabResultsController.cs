using System.Data;
using LcpUml3.Api.Data;
using LcpUml3.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabResultsController(HospitalContext context) : ControllerBase
{
    private readonly HospitalContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LabResult>>> GetAll(CancellationToken cancellationToken)
        => await _context.LabResults.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{id}")]
    public async Task<ActionResult<LabResult>> GetById(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.LabResults.AsNoTracking().FirstOrDefaultAsync(e => e.ResultId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<LabResult>> Post(LabResult result, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.LabResults.Add(result);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.ResultId }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, LabResult result, CancellationToken cancellationToken)
    {
        if (id != result.ResultId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(result).Property(e => e.RowVersion).OriginalValue = result.RowVersion;
        _context.Entry(result).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.LabResults.AnyAsync(e => e.ResultId == id, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected; refresh and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.LabResults.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.LabResults.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
