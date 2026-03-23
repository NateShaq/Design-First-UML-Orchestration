namespace AutonomousFleet.Api.Domain.Entities;

public record RoutePlanner
{
    public string Strategy { get; init; } = "EnergyOptimized";
    public List<Waypoint> Waypoints { get; init; } = new();
}

public record MissionPlan
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string VehicleId { get; init; } = string.Empty;
    public RoutePlanner Planner { get; init; } = new();
    public DateTimeOffset ScheduledStart { get; init; } = DateTimeOffset.UtcNow;
}

public record Waypoint
{
    public int Sequence { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double? HoldSeconds { get; init; }
}

public record TrafficUpdate
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string SegmentId { get; init; } = string.Empty;
    public string Condition { get; init; } = "FreeFlow";
    public DateTimeOffset ObservedAt { get; init; } = DateTimeOffset.UtcNow;
}

public record EdgeComputeNode
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Region { get; init; } = string.Empty;
    public int CpuCores { get; init; }
    public int GpuCores { get; init; }
}

public record ControlCommand
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string VehicleId { get; init; } = string.Empty;
    public string CommandType { get; init; } = "Pause";
    public string Payload { get; init; } = string.Empty;
    public DateTimeOffset IssuedAt { get; init; } = DateTimeOffset.UtcNow;
}

public record PassengerBooking
{
    public Guid Id { get; set; }
    public Passenger Passenger { get; set; } = new();
    public string VehicleId { get; set; } = string.Empty;
    public double PickupLat { get; set; }
    public double PickupLng { get; set; }
    public double DropoffLat { get; set; }
    public double DropoffLng { get; set; }
    public DateTimeOffset ScheduledFor { get; set; }
    public string Status { get; set; } = "Pending";
}

public record Passenger
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
}

public record RideRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string RequestedVehicleType { get; init; } = "SedanAV";
    public double PickupLat { get; init; }
    public double PickupLng { get; init; }
    public double DropoffLat { get; init; }
    public double DropoffLng { get; init; }
    public string Priority { get; init; } = "Standard";
}

public record Payment
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid BookingId { get; init; }
    public string Method { get; init; } = "Card";
    public decimal Amount { get; init; }
    public DateTimeOffset CapturedAt { get; init; } = DateTimeOffset.UtcNow;
}

public record FleetConfig
{
    public int MaxConcurrentMissions { get; init; } = 100;
    public int ChargingReservePercent { get; init; } = 15;
    public string Region { get; init; } = "default";
}
