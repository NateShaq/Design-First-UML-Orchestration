-- Smart Grid & Renewable Energy Distribution schema

CREATE TABLE SolarFarm (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    CapacityMw DECIMAL(12,2) NOT NULL,
    Location NVARCHAR(200) NOT NULL,
    AvailabilityFactor FLOAT NOT NULL
);

CREATE TABLE WindTurbine (
    Id INT IDENTITY PRIMARY KEY,
    Farm NVARCHAR(200) NOT NULL,
    RatedMw DECIMAL(12,2) NOT NULL,
    CurrentOutputMw DECIMAL(12,2) NOT NULL,
    WindSpeedMps FLOAT NOT NULL
);

CREATE TABLE HydroPlant (
    Id INT IDENTITY PRIMARY KEY,
    Reservoir NVARCHAR(200) NOT NULL,
    CapacityMw DECIMAL(12,2) NOT NULL,
    WaterLevelMeters DECIMAL(12,2) NOT NULL
);

CREATE TABLE GeothermalPlant (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    CapacityMw DECIMAL(12,2) NOT NULL,
    SteamPressureBar FLOAT NOT NULL
);

CREATE TABLE BiomassPlant (
    Id INT IDENTITY PRIMARY KEY,
    FuelType NVARCHAR(100) NOT NULL,
    CapacityMw DECIMAL(12,2) NOT NULL,
    MoistureContentPercent FLOAT NOT NULL
);

CREATE TABLE BatteryStorage (
    Id INT IDENTITY PRIMARY KEY,
    Site NVARCHAR(200) NOT NULL,
    CapacityMwh DECIMAL(12,2) NOT NULL,
    StateOfChargePercent DECIMAL(5,2) NOT NULL
);

CREATE TABLE EnergyStorageSystem (
    Id INT IDENTITY PRIMARY KEY,
    Type NVARCHAR(100) NOT NULL,
    CapacityMwh DECIMAL(12,2) NOT NULL,
    MaxDischargeMw DECIMAL(12,2) NOT NULL
);

CREATE TABLE SubstationMonitor (
    Id INT IDENTITY PRIMARY KEY,
    SubstationName NVARCHAR(200) NOT NULL,
    VoltageKv DECIMAL(12,2) NOT NULL,
    FrequencyHz DECIMAL(12,2) NOT NULL,
    LoadFactor FLOAT NOT NULL
);

CREATE TABLE LoadBalancer (
    Id INT IDENTITY PRIMARY KEY,
    Region NVARCHAR(100) NOT NULL,
    CurrentLoadMw DECIMAL(12,2) NOT NULL,
    TargetLoadMw DECIMAL(12,2) NOT NULL
);

CREATE TABLE DemandResponseEvent (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    StartTime DATETIME2 NOT NULL,
    EndTime DATETIME2 NOT NULL,
    ShedGoalMw DECIMAL(12,2) NOT NULL
);

CREATE TABLE GridStabilityIndex (
    Id INT IDENTITY PRIMARY KEY,
    Timestamp DATETIME2 NOT NULL,
    Value FLOAT NOT NULL,
    Status NVARCHAR(50) NOT NULL
);

CREATE TABLE GridAlert (
    Id INT IDENTITY PRIMARY KEY,
    RaisedAt DATETIME2 NOT NULL,
    Severity NVARCHAR(50) NOT NULL,
    Message NVARCHAR(500) NOT NULL
);

CREATE TABLE FaultRecord (
    Id INT IDENTITY PRIMARY KEY,
    Location NVARCHAR(200) NOT NULL,
    OccurredAt DATETIME2 NOT NULL,
    Cause NVARCHAR(200) NOT NULL
);

CREATE TABLE WeatherForecast (
    Id INT IDENTITY PRIMARY KEY,
    Region NVARCHAR(100) NOT NULL,
    ForecastDate DATE NOT NULL,
    WindSpeedMps FLOAT NOT NULL,
    SolarIrradianceWm2 FLOAT NOT NULL,
    TemperatureC FLOAT NOT NULL
);

CREATE TABLE ForecastAccuracy (
    Id INT IDENTITY PRIMARY KEY,
    ForecastId INT NOT NULL REFERENCES WeatherForecast(Id),
    ErrorPercent FLOAT NOT NULL,
    Metric NVARCHAR(50) NOT NULL
);

CREATE TABLE PowerGenerationRecord (
    Id INT IDENTITY PRIMARY KEY,
    SourceType NVARCHAR(100) NOT NULL,
    SourceId INT NOT NULL,
    Timestamp DATETIME2 NOT NULL,
    Megawatts DECIMAL(12,2) NOT NULL
);

CREATE TABLE PowerPurchaseAgreement (
    Id INT IDENTITY PRIMARY KEY,
    Counterparty NVARCHAR(200) NOT NULL,
    ContractMw DECIMAL(12,2) NOT NULL,
    PricePerMwh DECIMAL(12,2) NOT NULL,
    EffectiveDate DATE NOT NULL
);

CREATE TABLE CustomerAccount (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Address NVARCHAR(300) NOT NULL,
    Email NVARCHAR(200) NOT NULL
);

CREATE TABLE SmartMeter (
    Id INT IDENTITY PRIMARY KEY,
    CustomerAccountId INT NOT NULL REFERENCES CustomerAccount(Id),
    SerialNumber NVARCHAR(100) NOT NULL,
    InstalledOn DATE NOT NULL
);

CREATE TABLE MeterReading (
    Id INT IDENTITY PRIMARY KEY,
    SmartMeterId INT NOT NULL REFERENCES SmartMeter(Id),
    ReadAt DATETIME2 NOT NULL,
    Kwh DECIMAL(14,3) NOT NULL
);

CREATE TABLE TariffPlan (
    Id INT IDENTITY PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    RatePerKwh DECIMAL(12,4) NOT NULL,
    TimeOfUse BIT NOT NULL
);

CREATE TABLE BillingCycle (
    Id INT IDENTITY PRIMARY KEY,
    CustomerAccountId INT NOT NULL REFERENCES CustomerAccount(Id),
    PeriodStart DATE NOT NULL,
    PeriodEnd DATE NOT NULL
);

CREATE TABLE UsageBilling (
    Id INT IDENTITY PRIMARY KEY,
    BillingCycleId INT NOT NULL REFERENCES BillingCycle(Id),
    TotalKwh DECIMAL(14,3) NOT NULL,
    AmountDue DECIMAL(12,2) NOT NULL
);

CREATE TABLE Invoice (
    Id INT IDENTITY PRIMARY KEY,
    BillingCycleId INT NOT NULL REFERENCES BillingCycle(Id),
    IssuedOn DATE NOT NULL,
    DueDate DATE NOT NULL,
    Amount DECIMAL(12,2) NOT NULL
);

CREATE TABLE Payment (
    Id INT IDENTITY PRIMARY KEY,
    InvoiceId INT NOT NULL REFERENCES Invoice(Id),
    PaidOn DATE NOT NULL,
    Amount DECIMAL(12,2) NOT NULL,
    Method NVARCHAR(50) NOT NULL
);

CREATE TABLE OutageTicket (
    Id INT IDENTITY PRIMARY KEY,
    CustomerAccountId INT NOT NULL REFERENCES CustomerAccount(Id),
    OpenedAt DATETIME2 NOT NULL,
    Status NVARCHAR(50) NOT NULL
);

CREATE TABLE MaintenanceSchedule (
    Id INT IDENTITY PRIMARY KEY,
    AssetType NVARCHAR(100) NOT NULL,
    AssetId INT NOT NULL,
    ScheduledFor DATETIME2 NOT NULL,
    Crew NVARCHAR(100) NOT NULL
);

CREATE TABLE CrewAssignment (
    Id INT IDENTITY PRIMARY KEY,
    CrewName NVARCHAR(100) NOT NULL,
    Skillset NVARCHAR(200) NOT NULL,
    AssignedOn DATETIME2 NOT NULL
);

CREATE TABLE Transformer (
    Id INT IDENTITY PRIMARY KEY,
    Substation NVARCHAR(200) NOT NULL,
    RatingMva DECIMAL(12,2) NOT NULL,
    LoadPercent DECIMAL(5,2) NOT NULL
);

CREATE TABLE ElectricVehicleCharger (
    Id INT IDENTITY PRIMARY KEY,
    Location NVARCHAR(200) NOT NULL,
    Ports INT NOT NULL,
    MaxKw DECIMAL(12,2) NOT NULL
);
