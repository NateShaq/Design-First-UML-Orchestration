-- Smart City Traffic & Infrastructure Grid schema
-- Traffic Flow
CREATE TABLE TrafficSensor (
    Id UUID PRIMARY KEY,
    Location TEXT NOT NULL,
    Corridor TEXT NOT NULL,
    Active BOOLEAN NOT NULL,
    InstalledOn TIMESTAMP NOT NULL
);

CREATE TABLE SignalController (
    Id UUID PRIMARY KEY,
    Intersection TEXT NOT NULL,
    FirmwareVersion TEXT NOT NULL,
    AdaptiveEnabled BOOLEAN NOT NULL,
    LastSyncedUtc TIMESTAMP NOT NULL
);

CREATE TABLE CongestionEvent (
    Id UUID PRIMARY KEY,
    Corridor TEXT NOT NULL,
    Severity INT NOT NULL,
    DetectedUtc TIMESTAMP NOT NULL,
    Cause TEXT
);

CREATE TABLE TrafficFlowSnapshot (
    Corridor TEXT PRIMARY KEY,
    AverageSpeedMph DOUBLE PRECISION NOT NULL,
    VehicleCount INT NOT NULL,
    UpdatedAtUtc TIMESTAMP NOT NULL
);

-- Public Transit
CREATE TABLE TransitRoute (
    Id TEXT PRIMARY KEY,
    Mode TEXT NOT NULL,
    Name TEXT NOT NULL,
    HeadwayMinutes INT NOT NULL
);

CREATE TABLE TransitStop (
    Id UUID PRIMARY KEY,
    Name TEXT NOT NULL,
    RouteId TEXT NOT NULL REFERENCES TransitRoute(Id),
    Latitude DOUBLE PRECISION NOT NULL,
    Longitude DOUBLE PRECISION NOT NULL
);

CREATE TABLE ElectricBuses (
    FleetId UUID PRIMARY KEY,
    InService INT NOT NULL,
    Charging INT NOT NULL,
    OutOfService INT NOT NULL,
    SnapshotUtc TIMESTAMP NOT NULL
);

CREATE TABLE LightRail (
    TrainId UUID PRIMARY KEY,
    Line TEXT NOT NULL,
    Cars INT NOT NULL,
    Status TEXT NOT NULL
);

CREATE TABLE AutonomousShuttles (
    VehicleId UUID PRIMARY KEY,
    Zone TEXT NOT NULL,
    BatteryPercent DOUBLE PRECISION NOT NULL,
    InService BOOLEAN NOT NULL
);

CREATE TABLE RidershipStats (
    RouteId TEXT NOT NULL REFERENCES TransitRoute(Id),
    ServiceDate DATE NOT NULL,
    Boardings INT NOT NULL,
    Alightings INT NOT NULL,
    OnTimePerformance DOUBLE PRECISION NOT NULL,
    PRIMARY KEY(RouteId, ServiceDate)
);

CREATE TABLE ServiceAlert (
    Id UUID PRIMARY KEY,
    RouteId TEXT NOT NULL REFERENCES TransitRoute(Id),
    Message TEXT NOT NULL,
    EffectiveFromUtc TIMESTAMP NOT NULL,
    EffectiveToUtc TIMESTAMP
);

-- Emergency Response
CREATE TABLE DispatchCenter (
    Id UUID PRIMARY KEY,
    Name TEXT NOT NULL,
    CoverageArea TEXT NOT NULL,
    Active BOOLEAN NOT NULL
);

CREATE TABLE EmergencyUnit (
    Id UUID PRIMARY KEY,
    UnitType TEXT NOT NULL,
    Station TEXT NOT NULL,
    Available BOOLEAN NOT NULL,
    LastStatusUpdateUtc TIMESTAMP NOT NULL
);

CREATE TABLE HospitalCapacity (
    HospitalId UUID PRIMARY KEY,
    Name TEXT NOT NULL,
    ErBedsTotal INT NOT NULL,
    ErBedsAvailable INT NOT NULL,
    IcuBedsAvailable INT NOT NULL
);

CREATE TABLE ResponseTimeKPI (
    District TEXT PRIMARY KEY,
    AverageMinutes NUMERIC NOT NULL,
    NinetyPercentileMinutes NUMERIC NOT NULL,
    PeriodEndingUtc TIMESTAMP NOT NULL
);

CREATE TABLE AlertNotification (
    Id UUID PRIMARY KEY,
    Audience TEXT NOT NULL,
    Message TEXT NOT NULL,
    SentUtc TIMESTAMP NOT NULL,
    Channel TEXT NOT NULL
);

CREATE TABLE ResourceInventory (
    Id UUID PRIMARY KEY,
    ResourceType TEXT NOT NULL,
    Quantity INT NOT NULL,
    Location TEXT NOT NULL
);

-- Infrastructure / Grid
CREATE TABLE GridPowerMonitor (
    Id UUID PRIMARY KEY,
    Substation TEXT NOT NULL,
    LoadMva DOUBLE PRECISION NOT NULL,
    FrequencyHz DOUBLE PRECISION NOT NULL,
    SampledUtc TIMESTAMP NOT NULL
);

CREATE TABLE MaintenanceSchedule (
    Id UUID PRIMARY KEY,
    AssetType TEXT NOT NULL,
    AssetId TEXT NOT NULL,
    ScheduledFor TIMESTAMP NOT NULL,
    Crew TEXT NOT NULL,
    Notes TEXT
);

CREATE TABLE SensorHealth (
    SensorId UUID PRIMARY KEY,
    SensorType TEXT NOT NULL,
    Status TEXT NOT NULL,
    CheckedUtc TIMESTAMP NOT NULL
);
