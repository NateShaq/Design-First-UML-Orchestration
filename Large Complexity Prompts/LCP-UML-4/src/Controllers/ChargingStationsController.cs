using System.Data;
using FleetNetworkApi.Data;
using FleetNetworkApi.DTOs;
using FleetNetworkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetNetworkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChargingStationsController : ControllerBase
{
    private readonly FleetDbContext _db;
    public ChargingStationsController(FleetDbContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<ChargingStation>> Create([FromBody] ChargingStation station)
    {
        _db.ChargingStations.Add(station);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = station.Id }, station);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ChargingStation>> Get(Guid id)
    {
        var station = await _db.ChargingStations.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
        return station is null ? NotFound() : Ok(station);
    }

    [HttpPost("{id:guid}/reserve")]
    public async Task<IActionResult> Reserve(Guid id, SlotReserveDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var station = await _db.ChargingStations.FirstOrDefaultAsync(c => c.Id == id);
        if (station is null) return NotFound();
        if (station.SlotStatus != "available") return Conflict("Slot already reserved");
        if (!station.RowVersion.SequenceEqual(dto.Version.RowVersionBytes))
            return Conflict("Concurrent update detected");

        station.SlotStatus = "reserved";
        station.VehicleId = dto.VehicleId;
        station.LockToken = dto.Version.LockToken ?? station.LockToken;

        try
        {
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return Ok(station);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync();
            return Conflict("Ghost write prevented (rowversion mismatch)");
        }
    }
}
