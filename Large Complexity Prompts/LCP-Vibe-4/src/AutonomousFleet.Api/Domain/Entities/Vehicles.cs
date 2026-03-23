namespace AutonomousFleet.Api.Domain.Entities;

public record SedanAV
{
    public string VehicleId { get; init; } = string.Empty;
    public string Trim { get; init; } = "Urban";
    public BatteryPack Battery { get; init; } = new();
    public SensorPackage Sensors { get; init; } = new();
}

public record VanAV
{
    public string VehicleId { get; init; } = string.Empty;
    public int Seats { get; init; } = 6;
    public BatteryPack Battery { get; init; } = new();
    public SensorPackage Sensors { get; init; } = new();
}

public record DeliveryDrone
{
    public string VehicleId { get; init; } = string.Empty;
    public double MaxPayloadKg { get; init; } = 5.0;
    public BatteryPack Battery { get; init; } = new();
    public SensorPackage Sensors { get; init; } = new();
}

public record VehicleTelematics
{
    public string VehicleId { get; init; } = string.Empty;
    public TelematicsReading LatestReading { get; init; } = new();
    public List<TelematicsAlert> Alerts { get; init; } = new();
}

public record TelematicsReading
{
    public string VehicleId { get; init; } = string.Empty;
    public double SpeedKph { get; init; }
    public double BatteryPercent { get; init; }
    public double Latitude { get; init; }
    public double Longitude { get; init; }
    public double HeadingDegrees { get; init; }
    public DateTimeOffset Timestamp { get; init; } = DateTimeOffset.UtcNow;
}

public record TelematicsAlert
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string VehicleId { get; init; } = string.Empty;
    public string Severity { get; init; } = "info";
    public string Message { get; init; } = string.Empty;
    public DateTimeOffset RaisedAt { get; init; } = DateTimeOffset.UtcNow;
}

public record SensorPackage
{
    public LiDARProcessor LiDAR { get; init; } = new();
    public CameraSensor Cameras { get; init; } = new();
    public RadarSensor Radar { get; init; } = new();
    public GPSSensor GPS { get; init; } = new();
}

public record LiDARProcessor
{
    public string Model { get; init; } = "Velodyne-Alpha";
    public int PointsPerSecond { get; init; } = 1200000;
}

public record CameraSensor
{
    public string Model { get; init; } = "4K-HDR";
    public int Quantity { get; init; } = 6;
}

public record RadarSensor
{
    public string Model { get; init; } = "77GHz";
    public int RangeMeters { get; init; } = 200;
}

public record GPSSensor
{
    public string Constellation { get; init; } = "GPS+GLONASS";
    public double AccuracyMeters { get; init; } = 0.5;
}

public record BatteryPack
{
    public string Chemistry { get; init; } = "NMC";
    public int CapacityKWh { get; init; } = 90;
    public double StateOfHealthPercent { get; init; } = 98.5;
}

public record ChargingStation
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Name { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public int PortsAvailable { get; init; }
}

public record ChargingSession
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string VehicleId { get; init; } = string.Empty;
    public Guid StationId { get; init; }
    public DateTimeOffset StartedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? CompletedAt { get; init; }
    public double EnergyKWh { get; init; }
}

public record MaintenanceRecord
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string VehicleId { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public DateTimeOffset PerformedAt { get; init; } = DateTimeOffset.UtcNow;
}

public record IncidentReport
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string VehicleId { get; init; } = string.Empty;
    public string Category { get; init; } = "Safety";
    public string Summary { get; init; } = string.Empty;
    public string Severity { get; init; } = "Low";
    public DateTimeOffset ReportedAt { get; init; } = DateTimeOffset.UtcNow;
}
