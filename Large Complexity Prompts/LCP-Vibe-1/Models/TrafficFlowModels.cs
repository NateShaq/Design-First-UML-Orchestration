namespace LCP.Vibe.Api.Models;

public class TrafficSensor
{
    public Guid Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public string Corridor { get; set; } = string.Empty;
    public bool Active { get; set; }
    public DateTime InstalledOn { get; set; }
}

public class SignalController
{
    public Guid Id { get; set; }
    public string Intersection { get; set; } = string.Empty;
    public string FirmwareVersion { get; set; } = "1.0.0";
    public bool AdaptiveEnabled { get; set; }
    public DateTime LastSyncedUtc { get; set; }
}

public class CongestionEvent
{
    public Guid Id { get; set; }
    public string Corridor { get; set; } = string.Empty;
    public int Severity { get; set; }
    public DateTime DetectedUtc { get; set; }
    public string Cause { get; set; } = string.Empty;
}

public class RouteOptimizationRequest
{
    public Guid Id { get; set; }
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime RequestedUtc { get; set; }
    public string PreferredMode { get; set; } = "auto";
}

public class DynamicLaneConfig
{
    public Guid Id { get; set; }
    public string Corridor { get; set; } = string.Empty;
    public string Direction { get; set; } = "NB";
    public int LaneCount { get; set; }
    public DateTime EffectiveFromUtc { get; set; }
}

public class TrafficCamera
{
    public Guid Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public bool Recording { get; set; }
    public string StreamUrl { get; set; } = string.Empty;
}

public class VehicleCount
{
    public Guid SensorId { get; set; }
    public int Count { get; set; }
    public DateTime WindowStartUtc { get; set; }
    public DateTime WindowEndUtc { get; set; }
}

public class TravelTimeEstimate
{
    public string Corridor { get; set; } = string.Empty;
    public TimeSpan Typical { get; set; }
    public TimeSpan Current { get; set; }
    public DateTime GeneratedUtc { get; set; }
}

public class CorridorPerformance
{
    public string Corridor { get; set; } = string.Empty;
    public double ReliabilityIndex { get; set; }
    public double AverageSpeedMph { get; set; }
    public double DelayMinutesPerMile { get; set; }
    public DateTime PeriodEndingUtc { get; set; }
}

public class RoadWeatherSensor
{
    public Guid Id { get; set; }
    public string Location { get; set; } = string.Empty;
    public double SurfaceTempC { get; set; }
    public bool IcingDetected { get; set; }
    public DateTime SampledUtc { get; set; }
}

public class TrafficFlowSnapshot
{
    public string Corridor { get; set; } = string.Empty;
    public double AverageSpeedMph { get; set; }
    public int VehicleCount { get; set; }
    public DateTime UpdatedAtUtc { get; set; }
}
