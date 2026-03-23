using System.Data;
using FleetNetworkApi.Data;
using FleetNetworkApi.DTOs;
using FleetNetworkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetNetworkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly FleetDbContext _db;
    public BookingsController(FleetDbContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<Booking>> Create(BookingCreateDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var booking = new Booking
        {
            PassengerId = dto.PassengerId,
            PaymentAccountId = dto.PaymentAccountId,
            Status = "created",
            LockToken = Guid.NewGuid().ToString("N")
        };
        _db.Bookings.Add(booking);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return CreatedAtAction(nameof(GetById), new { id = booking.Id }, booking);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Booking>> GetById(Guid id)
    {
        var booking = await _db.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
        return booking is null ? NotFound() : Ok(booking);
    }

    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, BookingCancelDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == id);
        if (booking is null) return NotFound();
        if (!booking.RowVersion.SequenceEqual(dto.Version.RowVersionBytes))
            return Conflict("Concurrent update detected");

        booking.Status = "cancelled";
        try
        {
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return Ok(booking);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync();
            return Conflict("Ghost write prevented (rowversion mismatch)");
        }
    }
}
