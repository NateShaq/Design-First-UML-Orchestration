namespace LCP.Vibe.Api.Models;

public class GridPowerMonitor
{
    public Guid Id { get; set; }
    public string Substation { get; set; } = string.Empty;
    public double LoadMva { get; set; }
    public double FrequencyHz { get; set; }
    public DateTime SampledUtc { get; set; }
}

public class MaintenanceSchedule
{
    public Guid Id { get; set; }
    public string AssetType { get; set; } = string.Empty;
    public string AssetId { get; set; } = string.Empty;
    public DateTime ScheduledFor { get; set; }
    public string Crew { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class SensorHealth
{
    public Guid SensorId { get; set; }
    public string SensorType { get; set; } = string.Empty;
    public string Status { get; set; } = "Healthy";
    public DateTime CheckedUtc { get; set; }
}

public class PowerSubstation
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string FeedsCorridor { get; set; } = string.Empty;
    public bool BackupAvailable { get; set; }
}

public class ResilienceScore
{
    public string Zone { get; set; } = string.Empty;
    public double Score { get; set; }
    public DateTime CalculatedUtc { get; set; }
}

public class BackupGenerator
{
    public Guid Id { get; set; }
    public string Site { get; set; } = string.Empty;
    public double CapacityKw { get; set; }
    public bool TestedThisMonth { get; set; }
}

public class NetworkNode
{
    public Guid Id { get; set; }
    public string Role { get; set; } = "edge";
    public string Location { get; set; } = string.Empty;
    public bool Redundant { get; set; }
}

public class FiberLink
{
    public Guid Id { get; set; }
    public string FromNode { get; set; } = string.Empty;
    public string ToNode { get; set; } = string.Empty;
    public double BandwidthGbps { get; set; }
    public bool Lit { get; set; }
}

public class ControlRoom
{
    public Guid Id { get; set; }
    public string Facility { get; set; } = string.Empty;
    public string Supervisor { get; set; } = string.Empty;
    public bool RemoteCapable { get; set; }
}

public class CyberSecurityIncident
{
    public Guid Id { get; set; }
    public string System { get; set; } = string.Empty;
    public string Severity { get; set; } = "Low";
    public DateTime DetectedUtc { get; set; }
    public string MitigationStep { get; set; } = string.Empty;
}
