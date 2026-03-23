using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.Models;

public class IncidentReport : BaseEntity
{
    [Required]
    public Guid VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }

    public Guid? RoutePlanId { get; set; }
    public RoutePlan? RoutePlan { get; set; }

    [MaxLength(512)]
    public string Summary { get; set; } = string.Empty;

    [MaxLength(512)]
    public string? EvidenceUri { get; set; }

    public DateTimeOffset ReportedAt { get; set; } = DateTimeOffset.UtcNow;
}
