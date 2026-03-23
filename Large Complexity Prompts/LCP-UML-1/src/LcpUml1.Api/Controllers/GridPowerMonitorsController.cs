using System.Data;
using LcpUml1.Api.Data;
using LcpUml1.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GridPowerMonitorsController(SmartCityContext context) : ControllerBase
{
    private readonly SmartCityContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GridPowerMonitor>>> GetAll(CancellationToken cancellationToken)
        => await _context.GridPowerMonitors.AsNoTracking().ToListAsync(cancellationToken);

    [HttpPost]
    public async Task<ActionResult<GridPowerMonitor>> Post(GridPowerMonitor monitor, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.GridPowerMonitors.Add(monitor);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = monitor.GridPowerMonitorId }, monitor);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GridPowerMonitor>> Get(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.GridPowerMonitors.AsNoTracking()
            .FirstOrDefaultAsync(e => e.GridPowerMonitorId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, GridPowerMonitor monitor, CancellationToken cancellationToken)
    {
        if (id != monitor.GridPowerMonitorId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(monitor).Property(e => e.RowVersion).OriginalValue = monitor.RowVersion;
        _context.Entry(monitor).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.GridPowerMonitors.AnyAsync(e => e.GridPowerMonitorId == id, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected; refresh and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.GridPowerMonitors.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.GridPowerMonitors.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
