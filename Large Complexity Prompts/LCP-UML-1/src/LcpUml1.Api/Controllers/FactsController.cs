using LcpUml1.Api.Data;
using LcpUml1.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FactsController(SmartCityContext context) : ControllerBase
{
    private readonly SmartCityContext _context = context;

    [HttpPost("signal-events")]
    public async Task<ActionResult<SignalEventFact>> PostSignalEvent(SignalEventFact fact, CancellationToken cancellationToken)
    {
        _context.SignalEventFacts.Add(fact);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetSignalEvent), new { id = fact.EventId }, fact);
    }

    [HttpGet("signal-events/{id:int}")]
    public async Task<ActionResult<SignalEventFact>> GetSignalEvent(int id, CancellationToken cancellationToken)
    {
        var fact = await _context.SignalEventFacts.AsNoTracking()
            .Include(f => f.SignalController)
            .Include(f => f.Calendar)
            .FirstOrDefaultAsync(f => f.EventId == id, cancellationToken);
        return fact is null ? NotFound() : fact;
    }

    [HttpPost("power-events")]
    public async Task<ActionResult<PowerEventFact>> PostPowerEvent(PowerEventFact fact, CancellationToken cancellationToken)
    {
        _context.PowerEventFacts.Add(fact);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetPowerEvent), new { id = fact.EventId }, fact);
    }

    [HttpGet("power-events/{id:int}")]
    public async Task<ActionResult<PowerEventFact>> GetPowerEvent(int id, CancellationToken cancellationToken)
    {
        var fact = await _context.PowerEventFacts.AsNoTracking()
            .Include(f => f.GridPowerMonitor)
            .Include(f => f.Calendar)
            .FirstOrDefaultAsync(f => f.EventId == id, cancellationToken);
        return fact is null ? NotFound() : fact;
    }

    [HttpPost("maintenance-events")]
    public async Task<ActionResult<MaintenanceFact>> PostMaintenance(MaintenanceFact fact, CancellationToken cancellationToken)
    {
        _context.MaintenanceFacts.Add(fact);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetMaintenance), new { id = fact.MaintenanceId }, fact);
    }

    [HttpGet("maintenance-events/{id:int}")]
    public async Task<ActionResult<MaintenanceFact>> GetMaintenance(int id, CancellationToken cancellationToken)
    {
        var fact = await _context.MaintenanceFacts.AsNoTracking()
            .Include(f => f.Vehicle)
            .Include(f => f.Schedule)
            .Include(f => f.Calendar)
            .FirstOrDefaultAsync(f => f.MaintenanceId == id, cancellationToken);
        return fact is null ? NotFound() : fact;
    }
}
