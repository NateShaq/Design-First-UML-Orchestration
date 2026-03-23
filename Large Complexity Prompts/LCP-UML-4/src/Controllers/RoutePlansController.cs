using System.Data;
using FleetNetworkApi.Data;
using FleetNetworkApi.DTOs;
using FleetNetworkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetNetworkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutePlansController : ControllerBase
{
    private readonly FleetDbContext _db;
    public RoutePlansController(FleetDbContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<RoutePlan>> Create(RoutePlanCreateDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == dto.VehicleId);
        if (vehicle is null) return BadRequest("Vehicle not found");

        var plan = new RoutePlan
        {
            VehicleId = dto.VehicleId,
            MapRef = dto.MapRef,
            TrafficRef = dto.TrafficRef,
            LockToken = dto.LockToken ?? Guid.NewGuid().ToString("N")
        };
        _db.RoutePlans.Add(plan);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return CreatedAtAction(nameof(Get), new { id = plan.Id }, plan);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RoutePlan>> Get(Guid id)
    {
        var plan = await _db.RoutePlans.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
        return plan is null ? NotFound() : Ok(plan);
    }

    [HttpPost("{id:guid}/commit")]
    public async Task<IActionResult> Commit(Guid id, RoutePlanCommitDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var plan = await _db.RoutePlans.FirstOrDefaultAsync(r => r.Id == id);
        if (plan is null) return NotFound();
        if (!plan.RowVersion.SequenceEqual(dto.Version.RowVersionBytes))
            return Conflict("Concurrent update detected");

        if (!string.IsNullOrWhiteSpace(dto.MapRef)) plan.MapRef = dto.MapRef;
        if (!string.IsNullOrWhiteSpace(dto.TrafficRef)) plan.TrafficRef = dto.TrafficRef;
        plan.LockToken = dto.Version.LockToken ?? plan.LockToken;

        try
        {
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return Ok(plan);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync();
            return Conflict("Ghost write prevented (rowversion mismatch)");
        }
    }
}
