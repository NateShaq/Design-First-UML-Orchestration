using System.Data;
using FleetNetworkApi.Data;
using FleetNetworkApi.DTOs;
using FleetNetworkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetNetworkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly FleetDbContext _db;
    public MaintenanceController(FleetDbContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<MaintenanceSchedule>> Schedule(MaintenanceCreateDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var vehicleExists = await _db.Vehicles.AnyAsync(v => v.Id == dto.VehicleId);
        if (!vehicleExists) return BadRequest("Vehicle not found");

        var maintenance = new MaintenanceSchedule
        {
            VehicleId = dto.VehicleId,
            ScheduledAt = dto.ScheduledAt,
            Description = dto.Description,
            Status = "scheduled",
            LockToken = Guid.NewGuid().ToString("N")
        };
        _db.MaintenanceSchedules.Add(maintenance);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return CreatedAtAction(nameof(Get), new { id = maintenance.Id }, maintenance);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MaintenanceSchedule>> Get(Guid id)
    {
        var schedule = await _db.MaintenanceSchedules.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        return schedule is null ? NotFound() : Ok(schedule);
    }

    [HttpPost("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, MaintenanceUpdateDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var schedule = await _db.MaintenanceSchedules.FirstOrDefaultAsync(m => m.Id == id);
        if (schedule is null) return NotFound();
        if (!schedule.RowVersion.SequenceEqual(dto.Version.RowVersionBytes))
            return Conflict("Concurrent update detected");

        schedule.Status = dto.Status;
        schedule.LockToken = dto.Version.LockToken ?? schedule.LockToken;

        try
        {
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return Ok(schedule);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync();
            return Conflict("Ghost write prevented (rowversion mismatch)");
        }
    }
}
