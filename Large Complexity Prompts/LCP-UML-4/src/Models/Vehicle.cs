using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.Models;

public enum VehicleKind
{
    Sedan,
    Van,
    DeliveryDrone
}

public class Vehicle : BaseEntity
{
    [Required, MaxLength(64)]
    public string Vin { get; set; } = string.Empty;

    [MaxLength(32)]
    public string Status { get; set; } = "idle";

    [MaxLength(128)]
    public string Location { get; set; } = string.Empty;

    public VehicleKind Kind { get; set; }

    public RoutePlan? ActiveRoute { get; set; }

    public ICollection<TripAssignment> TripAssignments { get; set; } = new List<TripAssignment>();

    public ICollection<MaintenanceSchedule> MaintenanceSchedules { get; set; } = new List<MaintenanceSchedule>();

    public ICollection<IncidentReport> IncidentReports { get; set; } = new List<IncidentReport>();
}
