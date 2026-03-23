using System.Data;
using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Contracts.Responses;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using LcpUml6.Api.Services.Concurrency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/substation-monitors")]
public class SubstationMonitorsController : ControllerBase
{
    private readonly GridDbContext _context;
    private readonly ILogger<SubstationMonitorsController> _logger;

    public SubstationMonitorsController(GridDbContext context, ILogger<SubstationMonitorsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SubstationMonitor>>> GetAll() =>
        await _context.SubstationMonitors.AsNoTracking().ToListAsync();

    [HttpGet("{monitorId:guid}")]
    public async Task<ActionResult<SubstationMonitor>> Get(Guid monitorId)
    {
        var entity = await _context.SubstationMonitors.AsNoTracking().FirstOrDefaultAsync(x => x.MonitorId == monitorId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<SubstationMonitor>> Create([FromBody] CreateSubstationMonitorRequest request)
    {
        var entity = new SubstationMonitor
        {
            MonitorId = request.MonitorId ?? Guid.NewGuid(),
            GridConnectionId = request.GridConnectionId,
            BreakerState = request.BreakerState,
            Revision = 0
        };

        _context.SubstationMonitors.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { monitorId = entity.MonitorId }, entity);
    }

    [HttpPost("{monitorId:guid}/begin-update")]
    public async Task<ActionResult<BeginUpdateResponse>> BeginUpdate(Guid monitorId, [FromQuery] string? txId = null)
    {
        var entity = await _context.SubstationMonitors.AsNoTracking().FirstOrDefaultAsync(x => x.MonitorId == monitorId);
        if (entity is null) return NotFound();
        return new BeginUpdateResponse(ConcurrencyToken.Encode(entity.RowVersion), entity.Revision);
    }

    [HttpPost("{monitorId:guid}/commit-update")]
    public async Task<IActionResult> CommitUpdate(Guid monitorId, [FromBody] CommitUpdateRequest request)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var entity = await _context.SubstationMonitors.FirstOrDefaultAsync(x => x.MonitorId == monitorId);
        if (entity is null)
        {
            await tx.RollbackAsync();
            return NotFound();
        }

        _context.Entry(entity).Property(e => e.RowVersion).OriginalValue = ConcurrencyToken.Decode(request.Token);
        entity.Revision += 1;
        entity.BreakerState = request.Notes ?? entity.BreakerState;

        try
        {
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await tx.RollbackAsync();
            _logger.LogWarning(ex, "SubstationMonitor concurrency conflict for {MonitorId}", monitorId);
            return Conflict(new { message = "Stale token. Reload and retry." });
        }
    }

    [HttpPost("{monitorId:guid}/rollback-update")]
    public async Task<IActionResult> RollbackUpdate(Guid monitorId, [FromBody] CommitUpdateRequest request)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var entity = await _context.SubstationMonitors.FirstOrDefaultAsync(x => x.MonitorId == monitorId);
        if (entity is null)
        {
            await tx.RollbackAsync();
            return NotFound();
        }

        _context.Entry(entity).Property(e => e.RowVersion).OriginalValue = ConcurrencyToken.Decode(request.Token);
        await tx.RollbackAsync();
        return Ok(new { message = "Rolled back", revision = entity.Revision });
    }
}
