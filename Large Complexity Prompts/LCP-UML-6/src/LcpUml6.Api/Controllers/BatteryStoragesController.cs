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
[Route("api/battery-storages")]
public class BatteryStoragesController : ControllerBase
{
    private readonly GridDbContext _context;
    private readonly ILogger<BatteryStoragesController> _logger;

    public BatteryStoragesController(GridDbContext context, ILogger<BatteryStoragesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BatteryStorage>>> GetAll() =>
        await _context.BatteryStorages.AsNoTracking().ToListAsync();

    [HttpGet("{assetId:guid}")]
    public async Task<ActionResult<BatteryStorage>> Get(Guid assetId)
    {
        var entity = await _context.BatteryStorages.AsNoTracking().FirstOrDefaultAsync(x => x.AssetId == assetId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<BatteryStorage>> Create([FromBody] CreateBatteryStorageRequest request)
    {
        var entity = new BatteryStorage
        {
            AssetId = request.AssetId ?? Guid.NewGuid(),
            CapacityMwh = request.CapacityMwh,
            Chemistry = request.Chemistry,
            Notes = request.Notes,
            Revision = 0
        };

        _context.BatteryStorages.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { assetId = entity.AssetId }, entity);
    }

    [HttpPost("{assetId:guid}/begin-update")]
    public async Task<ActionResult<BeginUpdateResponse>> BeginUpdate(Guid assetId, [FromQuery] string? txId = null)
    {
        var entity = await _context.BatteryStorages.AsNoTracking().FirstOrDefaultAsync(x => x.AssetId == assetId);
        if (entity is null) return NotFound();
        return new BeginUpdateResponse(ConcurrencyToken.Encode(entity.RowVersion), entity.Revision);
    }

    [HttpPost("{assetId:guid}/commit-update")]
    public async Task<IActionResult> CommitUpdate(Guid assetId, [FromBody] CommitUpdateRequest request)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var entity = await _context.BatteryStorages.FirstOrDefaultAsync(x => x.AssetId == assetId);
        if (entity is null)
        {
            await tx.RollbackAsync();
            return NotFound();
        }

        _context.Entry(entity).Property(e => e.RowVersion).OriginalValue = ConcurrencyToken.Decode(request.Token);
        entity.Revision += 1;
        entity.Notes = request.Notes ?? entity.Notes;

        try
        {
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await tx.RollbackAsync();
            _logger.LogWarning(ex, "BatteryStorage concurrency conflict for {AssetId}", assetId);
            return Conflict(new { message = "Stale token. Reload and retry." });
        }
    }

    [HttpPost("{assetId:guid}/rollback-update")]
    public async Task<IActionResult> RollbackUpdate(Guid assetId, [FromBody] CommitUpdateRequest request)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var entity = await _context.BatteryStorages.FirstOrDefaultAsync(x => x.AssetId == assetId);
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
