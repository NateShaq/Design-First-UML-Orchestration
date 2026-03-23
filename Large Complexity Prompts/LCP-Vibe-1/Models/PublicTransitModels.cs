namespace LCP.Vibe.Api.Models;

public enum TransitMode
{
    ElectricBus,
    LightRail,
    AutonomousShuttle,
    MicroTransit
}

public class ElectricBuses
{
    public Guid FleetId { get; set; }
    public int InService { get; set; }
    public int Charging { get; set; }
    public int OutOfService { get; set; }
    public DateTime SnapshotUtc { get; set; }
}

public class LightRail
{
    public Guid TrainId { get; set; }
    public string Line { get; set; } = string.Empty;
    public int Cars { get; set; }
    public string Status { get; set; } = "OnTime";
}

public class AutonomousShuttles
{
    public Guid VehicleId { get; set; }
    public string Zone { get; set; } = string.Empty;
    public double BatteryPercent { get; set; }
    public bool InService { get; set; }
}

public class TransitRoute
{
    public string Id { get; set; } = string.Empty;
    public TransitMode Mode { get; set; }
    public string Name { get; set; } = string.Empty;
    public int HeadwayMinutes { get; set; }
}

public class TransitStop
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RouteId { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}

public class RidershipStats
{
    public string RouteId { get; set; } = string.Empty;
    public DateTime ServiceDate { get; set; }
    public int Boardings { get; set; }
    public int Alightings { get; set; }
    public double OnTimePerformance { get; set; }
}

public class FleetHealth
{
    public string FleetName { get; set; } = string.Empty;
    public int Vehicles { get; set; }
    public int Warnings { get; set; }
    public int CriticalAlerts { get; set; }
    public DateTime EvaluatedUtc { get; set; }
}

public class DriverSchedule
{
    public Guid Id { get; set; }
    public string DriverName { get; set; } = string.Empty;
    public string RouteId { get; set; } = string.Empty;
    public DateTime ShiftStart { get; set; }
    public DateTime ShiftEnd { get; set; }
}

public class FareCollection
{
    public Guid TransactionId { get; set; }
    public string RouteId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime CollectedUtc { get; set; }
    public string Method { get; set; } = "tap";
}

public class ServiceAlert
{
    public Guid Id { get; set; }
    public string RouteId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public DateTime EffectiveFromUtc { get; set; }
    public DateTime? EffectiveToUtc { get; set; }
}
