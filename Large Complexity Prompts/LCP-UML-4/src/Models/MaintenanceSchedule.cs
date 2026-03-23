using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.Models;

public class MaintenanceSchedule : BaseEntity
{
    [Required]
    public Guid VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    [MaxLength(256)]
    public string Description { get; set; } = string.Empty;

    public DateTimeOffset ScheduledAt { get; set; }

    [MaxLength(32)]
    public string Status { get; set; } = "scheduled";
}
