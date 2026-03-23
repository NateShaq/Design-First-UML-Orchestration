using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetNetworkApi.Models;

public class RoutePlan : BaseEntity
{
    [Required]
    public Guid VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    [MaxLength(128)]
    public string MapRef { get; set; } = string.Empty;

    [MaxLength(128)]
    public string TrafficRef { get; set; } = string.Empty;

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
