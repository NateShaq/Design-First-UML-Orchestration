-- Multi-National Manufacturing PLM schema (starter)

CREATE TABLE RawMaterial (
    Id INT PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    Specification NVARCHAR(80) NOT NULL,
    SupplierId INT NOT NULL
);

CREATE TABLE SubAssembly (
    Id INT PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    Version NVARCHAR(20) NOT NULL,
    BillOfMaterialsId INT NULL
);

CREATE TABLE FinishedProduct (
    Id INT PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    Sku NVARCHAR(60) NOT NULL,
    Revision NVARCHAR(20) NOT NULL,
    BillOfMaterialsId INT NULL
);

CREATE TABLE BillOfMaterials (
    Id INT PRIMARY KEY,
    Code NVARCHAR(50) NOT NULL,
    OwnerProductId INT NOT NULL
);

CREATE TABLE ChangeOrder (
    Id INT PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    RequestedBy NVARCHAR(150) NOT NULL,
    RequestedOn DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE ComplianceCert (
    Id INT PRIMARY KEY,
    CertificateType NVARCHAR(80) NOT NULL,
    IssuedOn DATE NOT NULL,
    ExpiresOn DATE NOT NULL,
    Issuer NVARCHAR(120) NOT NULL
);

CREATE TABLE SupplierContract (
    Id INT PRIMARY KEY,
    SupplierId INT NOT NULL,
    Terms NVARCHAR(MAX) NOT NULL,
    EffectiveDate DATE NOT NULL,
    ExpirationDate DATE NOT NULL
);

CREATE TABLE EngineeringDesign (
    Id INT PRIMARY KEY,
    DesignNumber NVARCHAR(80) NOT NULL,
    LeadEngineer NVARCHAR(120) NOT NULL,
    LifecyclePhase NVARCHAR(40) NOT NULL
);

CREATE TABLE ShopFloorExecution (
    Id INT PRIMARY KEY,
    Shift NVARCHAR(40) NOT NULL,
    Supervisor NVARCHAR(120) NOT NULL,
    Status NVARCHAR(40) NOT NULL
);

CREATE TABLE QualityAssurance (
    Id INT PRIMARY KEY,
    ProgramName NVARCHAR(120) NOT NULL,
    Manager NVARCHAR(120) NOT NULL,
    MaturityLevel NVARCHAR(40) NOT NULL
);

CREATE TABLE WorkOrder (
    Id INT PRIMARY KEY,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    ScheduledStart DATETIME2 NOT NULL,
    ScheduledEnd DATETIME2 NOT NULL,
    Status NVARCHAR(40) NOT NULL
);

CREATE TABLE RoutingStep (
    Id INT PRIMARY KEY,
    WorkOrderId INT NOT NULL,
    Operation NVARCHAR(120) NOT NULL,
    Sequence INT NOT NULL,
    MachineId INT NOT NULL
);

CREATE TABLE Machine (
    Id INT PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    Cell NVARCHAR(40) NOT NULL,
    Capability NVARCHAR(200) NOT NULL
);

CREATE TABLE Tooling (
    Id INT PRIMARY KEY,
    ToolNumber NVARCHAR(80) NOT NULL,
    Description NVARCHAR(200) NOT NULL,
    LastCalibration DATE NOT NULL
);

CREATE TABLE OperatorProfile (
    Id INT PRIMARY KEY,
    Name NVARCHAR(120) NOT NULL,
    BadgeId NVARCHAR(40) NOT NULL,
    QualificationLevel NVARCHAR(80) NOT NULL
);

CREATE TABLE InspectionPlan (
    Id INT PRIMARY KEY,
    PlanNumber NVARCHAR(80) NOT NULL,
    ProductId INT NOT NULL,
    SamplingMethod NVARCHAR(120) NOT NULL
);

CREATE TABLE InspectionResult (
    Id INT PRIMARY KEY,
    InspectionPlanId INT NOT NULL,
    WorkOrderId INT NOT NULL,
    Result NVARCHAR(40) NOT NULL,
    RecordedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

CREATE TABLE NonConformanceReport (
    Id INT PRIMARY KEY,
    WorkOrderId INT NOT NULL,
    Description NVARCHAR(400) NOT NULL,
    Severity NVARCHAR(40) NOT NULL
);

CREATE TABLE CorrectiveAction (
    Id INT PRIMARY KEY,
    NonConformanceId INT NOT NULL,
    Action NVARCHAR(400) NOT NULL,
    DueDate DATE NOT NULL,
    Owner NVARCHAR(120) NOT NULL
);

CREATE TABLE TestSpecification (
    Id INT PRIMARY KEY,
    SpecNumber NVARCHAR(80) NOT NULL,
    Description NVARCHAR(400) NOT NULL,
    Units NVARCHAR(20) NOT NULL
);

CREATE TABLE CalibrationRecord (
    Id INT PRIMARY KEY,
    ToolingId INT NOT NULL,
    PerformedOn DATE NOT NULL,
    Status NVARCHAR(40) NOT NULL
);

CREATE TABLE MaintenanceSchedule (
    Id INT PRIMARY KEY,
    MachineId INT NOT NULL,
    ScheduledFor DATE NOT NULL,
    Type NVARCHAR(60) NOT NULL
);

CREATE TABLE ProductionBatch (
    Id INT PRIMARY KEY,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    LotNumber NVARCHAR(60) NOT NULL
);

CREATE TABLE LotTracking (
    Id INT PRIMARY KEY,
    LotNumber NVARCHAR(60) NOT NULL,
    ManufacturedOn DATE NOT NULL,
    Expiration DATE NULL
);

CREATE TABLE InventoryItem (
    Id INT PRIMARY KEY,
    ItemNumber NVARCHAR(80) NOT NULL,
    OnHand INT NOT NULL,
    Reserved INT NOT NULL
);

CREATE TABLE WarehouseLocation (
    Id INT PRIMARY KEY,
    Code NVARCHAR(40) NOT NULL,
    Description NVARCHAR(200) NOT NULL
);

CREATE TABLE CustomerOrder (
    Id INT PRIMARY KEY,
    OrderNumber NVARCHAR(80) NOT NULL,
    RequestedShipDate DATE NOT NULL,
    Priority NVARCHAR(40) NOT NULL
);

CREATE TABLE Shipment (
    Id INT PRIMARY KEY,
    ShipmentNumber NVARCHAR(80) NOT NULL,
    ShippedOn DATE NOT NULL,
    Carrier NVARCHAR(120) NOT NULL
);

CREATE TABLE PackagingInstruction (
    Id INT PRIMARY KEY,
    InstructionNumber NVARCHAR(80) NOT NULL,
    Description NVARCHAR(400) NOT NULL
);

CREATE TABLE TraceabilityRecord (
    Id INT PRIMARY KEY,
    LotNumber NVARCHAR(60) NOT NULL,
    WorkOrderId INT NOT NULL,
    Evidence NVARCHAR(400) NOT NULL
);

-- Add foreign keys as needed per implementation
