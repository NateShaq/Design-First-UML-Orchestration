using FleetNetworkApi.Data;
using FleetNetworkApi.DTOs;
using FleetNetworkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetNetworkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly FleetDbContext _db;
    public VehiclesController(FleetDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles()
        => Ok(await _db.Vehicles.AsNoTracking().ToListAsync());

    [HttpPost]
    public async Task<ActionResult<Vehicle>> CreateVehicle([FromBody] VehicleCreateDto dto)
    {
        var vehicle = new Vehicle
        {
            Vin = dto.Vin,
            Kind = dto.Kind,
            Status = dto.Status ?? "idle",
            Location = dto.Location ?? string.Empty,
            LockToken = Guid.NewGuid().ToString("N")
        };
        _db.Vehicles.Add(vehicle);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetVehicles), new { id = vehicle.Id }, vehicle);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] VehicleUpdateDto dto)
    {
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == id);
        if (vehicle is null) return NotFound();

        if (!vehicle.RowVersion.SequenceEqual(dto.Version.RowVersionBytes))
            return Conflict("Concurrent update detected");

        vehicle.Status = dto.Status;
        if (!string.IsNullOrWhiteSpace(dto.Location))
            vehicle.Location = dto.Location;
        vehicle.LockToken = dto.Version.LockToken ?? vehicle.LockToken;

        try
        {
            await _db.SaveChangesAsync();
            return Ok(vehicle);
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("Ghost write prevented (rowversion mismatch)");
        }
    }
}
