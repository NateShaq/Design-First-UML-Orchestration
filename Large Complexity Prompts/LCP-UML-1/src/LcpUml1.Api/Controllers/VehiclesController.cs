using System.Data;
using LcpUml1.Api.Data;
using LcpUml1.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VehiclesController(SmartCityContext context) : ControllerBase
{
    private readonly SmartCityContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleDim>>> GetVehicles(CancellationToken cancellationToken)
        => await _context.Vehicles.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<VehicleDim>> GetVehicle(int id, CancellationToken cancellationToken)
    {
        var vehicle = await _context.Vehicles.AsNoTracking().FirstOrDefaultAsync(v => v.VehicleId == id, cancellationToken);
        return vehicle is null ? NotFound() : vehicle;
    }

    [HttpPost]
    public async Task<ActionResult<VehicleDim>> PostVehicle(VehicleDim vehicle, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Vehicles.Add(vehicle);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(GetVehicle), new { id = vehicle.VehicleId }, vehicle);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutVehicle(int id, VehicleDim vehicle, CancellationToken cancellationToken)
    {
        if (id != vehicle.VehicleId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(vehicle).Property(v => v.RowVersion).OriginalValue = vehicle.RowVersion;
        _context.Entry(vehicle).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.Vehicles.AnyAsync(v => v.VehicleId == id, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected. Refresh the resource and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteVehicle(int id, CancellationToken cancellationToken)
    {
        var vehicle = await _context.Vehicles.FindAsync(new object[] { id }, cancellationToken);
        if (vehicle is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Vehicles.Remove(vehicle);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
