using System.Data;
using FleetNetworkApi.Data;
using FleetNetworkApi.DTOs;
using FleetNetworkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetNetworkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripAssignmentsController : ControllerBase
{
    private readonly FleetDbContext _db;
    public TripAssignmentsController(FleetDbContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<TripAssignment>> Assign(TripAssignDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == dto.BookingId);
        var vehicle = await _db.Vehicles.FirstOrDefaultAsync(v => v.Id == dto.VehicleId);
        if (booking is null || vehicle is null) return BadRequest("Invalid booking or vehicle");

        var assignment = new TripAssignment
        {
            BookingId = dto.BookingId,
            VehicleId = dto.VehicleId,
            Status = "assigned",
            LockToken = Guid.NewGuid().ToString("N")
        };
        _db.TripAssignments.Add(assignment);
        booking.Status = "dispatched";

        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return CreatedAtAction(nameof(Get), new { id = assignment.Id }, assignment);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TripAssignment>> Get(Guid id)
    {
        var assignment = await _db.TripAssignments.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
        return assignment is null ? NotFound() : Ok(assignment);
    }

    [HttpPost("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, TripCommitDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var assignment = await _db.TripAssignments.FirstOrDefaultAsync(t => t.Id == id);
        if (assignment is null) return NotFound();
        if (!assignment.RowVersion.SequenceEqual(dto.Version.RowVersionBytes))
            return Conflict("Concurrent update detected");

        assignment.Status = dto.Status;
        assignment.LockToken = dto.Version.LockToken ?? assignment.LockToken;

        try
        {
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return Ok(assignment);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync();
            return Conflict("Ghost write prevented (rowversion mismatch)");
        }
    }
}
