-- Omnichannel Retail Enterprise Schema
CREATE TABLE Products (
    Id UUID PRIMARY KEY,
    Sku VARCHAR(40) UNIQUE NOT NULL,
    Name VARCHAR(120) NOT NULL,
    UnitPrice DECIMAL(12,2) NOT NULL,
    QuantityOnHand INT NOT NULL,
    Channel VARCHAR(40) NOT NULL,
    Type VARCHAR(30) NOT NULL,
    ExpirationDate TIMESTAMP NULL,
    ShelfLifeDays INT NULL,
    InsuranceValue DECIMAL(12,2) NULL,
    RequiresSignature BOOLEAN NULL,
    FileUrl VARCHAR(255) NULL,
    FileSizeMb INT NULL,
    LicenseKey VARCHAR(120) NULL
);

CREATE TABLE Customers (
    Id UUID PRIMARY KEY,
    Email VARCHAR(120) NOT NULL,
    FullName VARCHAR(120) NOT NULL,
    LoyaltyAccountId UUID NULL
);

CREATE TABLE LoyaltyPrograms (
    Id UUID PRIMARY KEY,
    Name VARCHAR(120) NOT NULL,
    EarnRate DECIMAL(6,4) NOT NULL,
    BurnRate DECIMAL(6,4) NOT NULL
);

CREATE TABLE LoyaltyAccounts (
    Id UUID PRIMARY KEY,
    ProgramId UUID REFERENCES LoyaltyPrograms(Id),
    PointsBalance INT NOT NULL DEFAULT 0
);

CREATE TABLE LoyaltyTransactions (
    Id UUID PRIMARY KEY,
    AccountId UUID REFERENCES LoyaltyAccounts(Id),
    SalesOrderId UUID NULL,
    PointsEarned INT NOT NULL DEFAULT 0,
    PointsRedeemed INT NOT NULL DEFAULT 0,
    Timestamp TIMESTAMP NOT NULL DEFAULT NOW()
);

CREATE TABLE SalesOrders (
    Id UUID PRIMARY KEY,
    CustomerId UUID REFERENCES Customers(Id),
    OrderedAt TIMESTAMP NOT NULL,
    Channel VARCHAR(40) NOT NULL,
    TaxTotal DECIMAL(12,2) NOT NULL DEFAULT 0,
    GrandTotal DECIMAL(12,2) NOT NULL DEFAULT 0
);

CREATE TABLE OrderLines (
    SalesOrderId UUID REFERENCES SalesOrders(Id),
    LineNumber INT NOT NULL,
    ProductId UUID REFERENCES Products(Id),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(12,2) NOT NULL,
    TaxAmount DECIMAL(12,2) NOT NULL DEFAULT 0,
    PRIMARY KEY (SalesOrderId, LineNumber)
);

CREATE TABLE Payments (
    Id UUID PRIMARY KEY,
    SalesOrderId UUID REFERENCES SalesOrders(Id),
    Method VARCHAR(30) NOT NULL,
    Amount DECIMAL(12,2) NOT NULL,
    PaidAt TIMESTAMP NOT NULL
);

CREATE TABLE ReturnAuthorizations (
    Id UUID PRIMARY KEY,
    SalesOrderId UUID REFERENCES SalesOrders(Id),
    CreatedAt TIMESTAMP NOT NULL,
    ReasonCode VARCHAR(60) NOT NULL
);

CREATE TABLE ReturnLineItems (
    ReturnAuthorizationId UUID REFERENCES ReturnAuthorizations(Id),
    LineNumber INT NOT NULL,
    ProductId UUID REFERENCES Products(Id),
    Quantity INT NOT NULL,
    Condition VARCHAR(40) NOT NULL,
    PRIMARY KEY (ReturnAuthorizationId, LineNumber)
);

CREATE TABLE InventoryAudits (
    Id UUID PRIMARY KEY,
    ProductId UUID REFERENCES Products(Id),
    Timestamp TIMESTAMP NOT NULL,
    Change INT NOT NULL,
    Reason VARCHAR(80) NOT NULL
);

CREATE TABLE Warehouses (
    Id UUID PRIMARY KEY,
    Code VARCHAR(20) NOT NULL,
    Address VARCHAR(200) NOT NULL
);

CREATE TABLE StockMovements (
    Id UUID PRIMARY KEY,
    ProductId UUID REFERENCES Products(Id),
    WarehouseId UUID REFERENCES Warehouses(Id),
    Quantity INT NOT NULL,
    Direction VARCHAR(20) NOT NULL,
    OccurredAt TIMESTAMP NOT NULL
);

CREATE TABLE Shipments (
    Id UUID PRIMARY KEY,
    SalesOrderId UUID REFERENCES SalesOrders(Id),
    Carrier VARCHAR(60) NOT NULL,
    TrackingNumber VARCHAR(80),
    ShippedAt TIMESTAMP NOT NULL,
    DeliveredAt TIMESTAMP NULL
);

CREATE TABLE DeliveryRoutes (
    Id UUID PRIMARY KEY,
    RouteCode VARCHAR(40) NOT NULL
);

CREATE TABLE DeliveryRouteShipments (
    RouteId UUID REFERENCES DeliveryRoutes(Id),
    ShipmentId UUID REFERENCES Shipments(Id),
    PRIMARY KEY (RouteId, ShipmentId)
);

CREATE TABLE Promotions (
    Id UUID PRIMARY KEY,
    Name VARCHAR(120) NOT NULL,
    DiscountAmount DECIMAL(12,2) NOT NULL,
    IsPercentage BOOLEAN NOT NULL,
    ValidFrom TIMESTAMP NOT NULL,
    ValidTo TIMESTAMP NOT NULL
);

CREATE TABLE PricingRules (
    Id UUID PRIMARY KEY,
    Name VARCHAR(120) NOT NULL,
    AppliesToChannel VARCHAR(40) NOT NULL,
    Adjustment DECIMAL(8,2) NOT NULL,
    IsPercentage BOOLEAN NOT NULL
);

CREATE TABLE Stores (
    Id UUID PRIMARY KEY,
    Name VARCHAR(120) NOT NULL,
    Location VARCHAR(200) NOT NULL
);

CREATE TABLE PointOfSaleTerminals (
    Id UUID PRIMARY KEY,
    StoreId UUID REFERENCES Stores(Id),
    TerminalCode VARCHAR(40) NOT NULL,
    SoftwareVersion VARCHAR(40) NOT NULL
);

CREATE TABLE ChannelMappings (
    Id UUID PRIMARY KEY,
    ExternalChannel VARCHAR(40) NOT NULL,
    InternalChannel VARCHAR(40) NOT NULL,
    SynchronizationMode VARCHAR(30) NOT NULL
);

CREATE TABLE FulfillmentRequests (
    Id UUID PRIMARY KEY,
    SalesOrderId UUID REFERENCES SalesOrders(Id),
    WarehouseId UUID REFERENCES Warehouses(Id),
    Status VARCHAR(30) NOT NULL
);
