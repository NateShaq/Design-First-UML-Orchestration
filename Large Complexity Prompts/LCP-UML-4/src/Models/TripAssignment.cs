using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.Models;

public class TripAssignment : BaseEntity
{
    [Required]
    public Guid BookingId { get; set; }
    public Booking? Booking { get; set; }

    [Required]
    public Guid VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    [MaxLength(32)]
    public string Status { get; set; } = "pending";

    public DateTimeOffset AssignedAt { get; set; } = DateTimeOffset.UtcNow;
}
