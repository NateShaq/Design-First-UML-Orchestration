-- Initial SQL Server schema aligned to LCP-UML-6
-- 3NF tables with rowversion-based ghost-write protection

CREATE TABLE GridConnections (
    GridConnectionId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Node NVARCHAR(200) NOT NULL
);

CREATE TABLE WeatherStations (
    WeatherStationId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Location NVARCHAR(200) NOT NULL
);

CREATE TABLE MaintenanceSchedules (
    MaintenanceScheduleId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    NextServiceDate DATETIME2 NOT NULL,
    Notes NVARCHAR(4000) NULL
);

CREATE TABLE Inverters (
    InverterId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Model NVARCHAR(120) NOT NULL,
    FirmwareVersion NVARCHAR(50) NOT NULL
);

CREATE TABLE BatteryStorages (
    AssetId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    CapacityMwh DECIMAL(18,4) NOT NULL,
    Chemistry NVARCHAR(100) NOT NULL,
    Notes NVARCHAR(4000) NULL,
    Revision INT NOT NULL DEFAULT 0,
    RowVersion ROWVERSION NOT NULL
);

CREATE TABLE SolarFarms (
    AssetId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    GridConnectionId UNIQUEIDENTIFIER NULL REFERENCES GridConnections(GridConnectionId),
    WeatherStationId UNIQUEIDENTIFIER NULL REFERENCES WeatherStations(WeatherStationId),
    BatteryStorageId UNIQUEIDENTIFIER NULL REFERENCES BatteryStorages(AssetId),
    InverterId UNIQUEIDENTIFIER NULL REFERENCES Inverters(InverterId),
    MaintenanceScheduleId UNIQUEIDENTIFIER NULL REFERENCES MaintenanceSchedules(MaintenanceScheduleId),
    Notes NVARCHAR(4000) NULL,
    Revision INT NOT NULL DEFAULT 0,
    RowVersion ROWVERSION NOT NULL
);

CREATE TABLE WindTurbines (
    AssetId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    GridConnectionId UNIQUEIDENTIFIER NULL REFERENCES GridConnections(GridConnectionId),
    MaintenanceScheduleId UNIQUEIDENTIFIER NULL REFERENCES MaintenanceSchedules(MaintenanceScheduleId),
    InverterId UNIQUEIDENTIFIER NULL REFERENCES Inverters(InverterId),
    Notes NVARCHAR(4000) NULL,
    Revision INT NOT NULL DEFAULT 0,
    RowVersion ROWVERSION NOT NULL
);

CREATE TABLE HydroPlants (
    AssetId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    GridConnectionId UNIQUEIDENTIFIER NULL REFERENCES GridConnections(GridConnectionId),
    MaintenanceScheduleId UNIQUEIDENTIFIER NULL REFERENCES MaintenanceSchedules(MaintenanceScheduleId),
    Notes NVARCHAR(4000) NULL,
    Revision INT NOT NULL DEFAULT 0,
    RowVersion ROWVERSION NOT NULL
);

CREATE TABLE SubstationMonitors (
    MonitorId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    GridConnectionId UNIQUEIDENTIFIER NULL REFERENCES GridConnections(GridConnectionId),
    BreakerState NVARCHAR(200) NULL,
    Revision INT NOT NULL DEFAULT 0,
    RowVersion ROWVERSION NOT NULL
);

CREATE TABLE LoadBalancers (
    BalancerId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    GridConnectionId UNIQUEIDENTIFIER NULL REFERENCES GridConnections(GridConnectionId),
    DispatchPlan NVARCHAR(200) NULL,
    Revision INT NOT NULL DEFAULT 0,
    RowVersion ROWVERSION NOT NULL
);

CREATE TABLE SmartMeters (
    MeterId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    CustomerId UNIQUEIDENTIFIER NOT NULL,
    GridConnectionId UNIQUEIDENTIFIER NULL REFERENCES GridConnections(GridConnectionId)
);

CREATE TABLE TariffPlans (
    TariffPlanId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    RateStructure NVARCHAR(4000) NOT NULL
);

CREATE TABLE SettlementEngines (
    SettlementId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Status NVARCHAR(80) NOT NULL
);

CREATE TABLE UsageBillings (
    BillingId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    MeterId UNIQUEIDENTIFIER NOT NULL REFERENCES SmartMeters(MeterId),
    TariffPlanId UNIQUEIDENTIFIER NOT NULL REFERENCES TariffPlans(TariffPlanId),
    SettlementId UNIQUEIDENTIFIER NOT NULL REFERENCES SettlementEngines(SettlementId),
    PeriodStart DATETIME2 NOT NULL,
    PeriodEnd DATETIME2 NOT NULL,
    MeasuredKwh DECIMAL(18,4) NOT NULL,
    Notes NVARCHAR(4000) NULL,
    Revision INT NOT NULL DEFAULT 0,
    RowVersion ROWVERSION NOT NULL
);

CREATE TABLE ForecastServices (
    ForecastServiceId UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Provider NVARCHAR(120) NOT NULL,
    WeatherStationId UNIQUEIDENTIFIER NOT NULL REFERENCES WeatherStations(WeatherStationId)
);
