using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.Models;

public class Booking : BaseEntity
{
    [Required]
    public Guid PassengerId { get; set; }
    public PassengerProfile? Passenger { get; set; }

    [Required]
    public Guid PaymentAccountId { get; set; }
    public PaymentAccount? PaymentAccount { get; set; }

    [MaxLength(32)]
    public string Status { get; set; } = "created";

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
