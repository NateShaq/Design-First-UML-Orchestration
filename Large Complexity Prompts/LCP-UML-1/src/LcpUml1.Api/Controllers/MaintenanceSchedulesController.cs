using System.Data;
using LcpUml1.Api.Data;
using LcpUml1.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceSchedulesController(SmartCityContext context) : ControllerBase
{
    private readonly SmartCityContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaintenanceSchedule>>> GetAll(CancellationToken cancellationToken)
        => await _context.MaintenanceSchedules.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MaintenanceSchedule>> Get(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.MaintenanceSchedules.AsNoTracking()
            .FirstOrDefaultAsync(e => e.MaintenanceScheduleId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<MaintenanceSchedule>> Post(MaintenanceSchedule schedule, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.MaintenanceSchedules.Add(schedule);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = schedule.MaintenanceScheduleId }, schedule);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, MaintenanceSchedule schedule, CancellationToken cancellationToken)
    {
        if (id != schedule.MaintenanceScheduleId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(schedule).Property(e => e.RowVersion).OriginalValue = schedule.RowVersion;
        _context.Entry(schedule).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.MaintenanceSchedules.AnyAsync(e => e.MaintenanceScheduleId == id, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected; refresh and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.MaintenanceSchedules.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.MaintenanceSchedules.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
