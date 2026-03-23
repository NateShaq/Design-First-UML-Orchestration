-- Autonomous Vehicle Fleet Network schema

CREATE TABLE Vehicles (
    VehicleId VARCHAR(32) PRIMARY KEY,
    Type VARCHAR(20) NOT NULL,
    Trim VARCHAR(32),
    Seats INT,
    MaxPayloadKg DECIMAL(6,2),
    BatteryCapacityKWh INT,
    BatteryChemistry VARCHAR(16),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE SensorPackages (
    VehicleId VARCHAR(32) PRIMARY KEY REFERENCES Vehicles(VehicleId),
    LiDARModel VARCHAR(64),
    LiDARPointsPerSecond INT,
    CameraModel VARCHAR(64),
    CameraQuantity INT,
    RadarModel VARCHAR(64),
    RadarRangeMeters INT,
    GPSConstellation VARCHAR(64),
    GPSAccuracyMeters DECIMAL(6,2)
);

CREATE TABLE TelematicsReadings (
    Id UUID PRIMARY KEY,
    VehicleId VARCHAR(32) REFERENCES Vehicles(VehicleId),
    SpeedKph DECIMAL(6,2),
    BatteryPercent DECIMAL(5,2),
    Latitude DECIMAL(10,6),
    Longitude DECIMAL(10,6),
    HeadingDegrees DECIMAL(6,2),
    RecordedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX IX_TelematicsReadings_Vehicle_RecordedAt ON TelematicsReadings (VehicleId, RecordedAt DESC);

CREATE TABLE TelematicsAlerts (
    Id UUID PRIMARY KEY,
    VehicleId VARCHAR(32) REFERENCES Vehicles(VehicleId),
    Severity VARCHAR(16),
    Message TEXT,
    RaisedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ChargingStations (
    Id UUID PRIMARY KEY,
    Name VARCHAR(100),
    Location VARCHAR(255),
    PortsAvailable INT
);

CREATE TABLE ChargingSessions (
    Id UUID PRIMARY KEY,
    VehicleId VARCHAR(32) REFERENCES Vehicles(VehicleId),
    StationId UUID REFERENCES ChargingStations(Id),
    StartedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    CompletedAt TIMESTAMP,
    EnergyKWh DECIMAL(8,2)
);

CREATE TABLE Passengers (
    Id UUID PRIMARY KEY,
    Name VARCHAR(120) NOT NULL,
    Phone VARCHAR(40)
);

CREATE TABLE Bookings (
    Id UUID PRIMARY KEY,
    PassengerId UUID REFERENCES Passengers(Id),
    VehicleId VARCHAR(32) REFERENCES Vehicles(VehicleId),
    PickupLat DECIMAL(10,6),
    PickupLng DECIMAL(10,6),
    DropoffLat DECIMAL(10,6),
    DropoffLng DECIMAL(10,6),
    ScheduledFor TIMESTAMP,
    Status VARCHAR(24)
);

CREATE TABLE Payments (
    Id UUID PRIMARY KEY,
    BookingId UUID REFERENCES Bookings(Id),
    Method VARCHAR(24),
    Amount DECIMAL(10,2),
    CapturedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE RoutePlans (
    Id UUID PRIMARY KEY,
    VehicleId VARCHAR(32) REFERENCES Vehicles(VehicleId),
    Strategy VARCHAR(32),
    ScheduledStart TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Waypoints (
    RoutePlanId UUID REFERENCES RoutePlans(Id),
    Sequence INT,
    Latitude DECIMAL(10,6),
    Longitude DECIMAL(10,6),
    HoldSeconds INT,
    PRIMARY KEY (RoutePlanId, Sequence)
);

CREATE TABLE RemoteOperators (
    Id UUID PRIMARY KEY,
    Name VARCHAR(120),
    CertificationLevel VARCHAR(8)
);

CREATE TABLE OperatorSupportSessions (
    Id UUID PRIMARY KEY,
    OperatorId UUID REFERENCES RemoteOperators(Id),
    VehicleId VARCHAR(32) REFERENCES Vehicles(VehicleId),
    StartedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(24)
);

CREATE TABLE RemoteAssistanceRequests (
    Id UUID PRIMARY KEY,
    VehicleId VARCHAR(32) REFERENCES Vehicles(VehicleId),
    Reason VARCHAR(120),
    Priority VARCHAR(16),
    ContextSnapshotUrl TEXT,
    RequestedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE MaintenanceRecords (
    Id UUID PRIMARY KEY,
    VehicleId VARCHAR(32) REFERENCES Vehicles(VehicleId),
    Description TEXT,
    PerformedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE IncidentReports (
    Id UUID PRIMARY KEY,
    VehicleId VARCHAR(32) REFERENCES Vehicles(VehicleId),
    Category VARCHAR(40),
    Summary TEXT,
    Severity VARCHAR(16),
    ReportedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);
