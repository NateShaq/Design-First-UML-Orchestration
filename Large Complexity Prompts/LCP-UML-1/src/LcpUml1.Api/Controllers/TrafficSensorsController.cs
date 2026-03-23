using System.Data;
using LcpUml1.Api.Data;
using LcpUml1.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrafficSensorsController(SmartCityContext context) : ControllerBase
{
    private readonly SmartCityContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrafficSensor>>> GetAll(CancellationToken cancellationToken)
        => await _context.TrafficSensors.AsNoTracking().ToListAsync(cancellationToken);

    [HttpPost]
    public async Task<ActionResult<TrafficSensor>> Post(TrafficSensor sensor, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.TrafficSensors.Add(sensor);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = sensor.TrafficSensorId }, sensor);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrafficSensor>> GetById(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.TrafficSensors.AsNoTracking()
            .FirstOrDefaultAsync(e => e.TrafficSensorId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, TrafficSensor sensor, CancellationToken cancellationToken)
    {
        if (id != sensor.TrafficSensorId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(sensor).Property(e => e.RowVersion).OriginalValue = sensor.RowVersion;
        _context.Entry(sensor).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.TrafficSensors.AnyAsync(e => e.TrafficSensorId == id, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected; refresh and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.TrafficSensors.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.TrafficSensors.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
