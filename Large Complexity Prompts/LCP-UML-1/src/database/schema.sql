-- Smart City Traffic & Infrastructure Grid (LCP-UML-1)
-- 3NF schema with rowversion ghost-write protection

CREATE TABLE RouteDim (
    RouteId INT IDENTITY PRIMARY KEY,
    Mode NVARCHAR(64) NOT NULL
);

CREATE TABLE VehicleDim (
    VehicleId INT IDENTITY PRIMARY KEY,
    VehicleType INT NOT NULL,
    RouteId INT NULL,
    RowVersion ROWVERSION NOT NULL,
    CONSTRAINT FK_VehicleDim_RouteDim FOREIGN KEY (RouteId) REFERENCES RouteDim(RouteId)
);

CREATE TABLE Intersection (
    IntersectionId INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(128) NOT NULL
);

CREATE TABLE TrafficSensor (
    TrafficSensorId INT IDENTITY PRIMARY KEY,
    SensorType NVARCHAR(64) NOT NULL,
    IntersectionId INT NULL,
    Status NVARCHAR(32) NOT NULL DEFAULT 'Active',
    RowVersion ROWVERSION NOT NULL,
    CONSTRAINT FK_TrafficSensor_Intersection FOREIGN KEY (IntersectionId) REFERENCES Intersection(IntersectionId)
);

CREATE TABLE SignalController (
    SignalControllerId INT IDENTITY PRIMARY KEY,
    ControllerName NVARCHAR(128) NOT NULL,
    IntersectionId INT NULL,
    State NVARCHAR(32) NOT NULL DEFAULT 'Unknown',
    RowVersion ROWVERSION NOT NULL,
    CONSTRAINT FK_SignalController_Intersection FOREIGN KEY (IntersectionId) REFERENCES Intersection(IntersectionId)
);

CREATE TABLE GridPowerMonitor (
    GridPowerMonitorId INT IDENTITY PRIMARY KEY,
    Location NVARCHAR(128) NOT NULL,
    CurrentLoadKw DECIMAL(10,2) NOT NULL DEFAULT 0,
    RowVersion ROWVERSION NOT NULL
);

CREATE TABLE MaintenanceSchedule (
    MaintenanceScheduleId INT IDENTITY PRIMARY KEY,
    VehicleId INT NOT NULL,
    NextServiceDate DATE NULL,
    Status NVARCHAR(32) NOT NULL DEFAULT 'Planned',
    RowVersion ROWVERSION NOT NULL,
    CONSTRAINT FK_MaintenanceSchedule_VehicleDim FOREIGN KEY (VehicleId) REFERENCES VehicleDim(VehicleId)
);

CREATE TABLE CalendarDim (
    DateId INT IDENTITY PRIMARY KEY,
    Ymd DATE NOT NULL UNIQUE
);

CREATE TABLE SensorDim (
    SensorId INT IDENTITY PRIMARY KEY,
    IntersectionId INT NULL,
    CONSTRAINT FK_SensorDim_Intersection FOREIGN KEY (IntersectionId) REFERENCES Intersection(IntersectionId)
);

CREATE TABLE SignalEventFact (
    EventId INT IDENTITY PRIMARY KEY,
    SignalControllerId INT NOT NULL,
    DateId INT NOT NULL,
    Timestamp DATETIME2 NOT NULL,
    State NVARCHAR(32) NOT NULL,
    CONSTRAINT FK_SignalEventFact_Controller FOREIGN KEY (SignalControllerId) REFERENCES SignalController(SignalControllerId) ON DELETE NO ACTION,
    CONSTRAINT FK_SignalEventFact_Date FOREIGN KEY (DateId) REFERENCES CalendarDim(DateId) ON DELETE NO ACTION
);

CREATE TABLE PowerEventFact (
    EventId INT IDENTITY PRIMARY KEY,
    GridPowerMonitorId INT NOT NULL,
    DateId INT NOT NULL,
    Timestamp DATETIME2 NOT NULL,
    LoadKw DECIMAL(10,2) NOT NULL,
    CONSTRAINT FK_PowerEventFact_Monitor FOREIGN KEY (GridPowerMonitorId) REFERENCES GridPowerMonitor(GridPowerMonitorId) ON DELETE NO ACTION,
    CONSTRAINT FK_PowerEventFact_Date FOREIGN KEY (DateId) REFERENCES CalendarDim(DateId) ON DELETE NO ACTION
);

CREATE TABLE MaintenanceFact (
    MaintenanceId INT IDENTITY PRIMARY KEY,
    VehicleId INT NOT NULL,
    ScheduleId INT NOT NULL,
    DateId INT NOT NULL,
    Status NVARCHAR(32) NOT NULL,
    CONSTRAINT FK_MaintenanceFact_Vehicle FOREIGN KEY (VehicleId) REFERENCES VehicleDim(VehicleId) ON DELETE NO ACTION,
    CONSTRAINT FK_MaintenanceFact_Schedule FOREIGN KEY (ScheduleId) REFERENCES MaintenanceSchedule(MaintenanceScheduleId) ON DELETE NO ACTION,
    CONSTRAINT FK_MaintenanceFact_Date FOREIGN KEY (DateId) REFERENCES CalendarDim(DateId) ON DELETE NO ACTION
);

-- Indexes to keep fact loads fast and maintain 3NF lookup performance
CREATE INDEX IX_SignalEventFact_ControllerDate ON SignalEventFact(SignalControllerId, DateId);
CREATE INDEX IX_PowerEventFact_MonitorDate ON PowerEventFact(GridPowerMonitorId, DateId);
CREATE INDEX IX_MaintenanceFact_VehicleDate ON MaintenanceFact(VehicleId, DateId);
