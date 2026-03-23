-- Government Digital Identity & Public Services schema
-- Key domains: IdentityRegistry, TaxCollection, SocialSecurityBenefits

CREATE TABLE Citizens (
    Id UUID PRIMARY KEY,
    NationalId VARCHAR(50) NOT NULL UNIQUE,
    FullName VARCHAR(200) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Email VARCHAR(200),
    Phone VARCHAR(50)
);

CREATE TABLE IdentityRecords (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    CreatedAt TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(50) NOT NULL
);

CREATE TABLE IdentityRegistry (
    Id UUID PRIMARY KEY,
    RegistryName VARCHAR(200) NOT NULL
);

CREATE TABLE RegistryEntries (
    RegistryId UUID NOT NULL REFERENCES IdentityRegistry(Id),
    IdentityRecordId UUID NOT NULL REFERENCES IdentityRecords(Id),
    PRIMARY KEY (RegistryId, IdentityRecordId)
);

CREATE TABLE BiometricData (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    Modality VARCHAR(50) NOT NULL,
    Hash VARCHAR(200) NOT NULL,
    CapturedAt TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE PublicRecords (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    RecordType VARCHAR(100) NOT NULL,
    Data JSONB,
    UpdatedAt TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Jurisdictions (
    Id UUID PRIMARY KEY,
    Name VARCHAR(150) NOT NULL,
    RegionCode VARCHAR(20) NOT NULL
);

CREATE TABLE AuditTrails (
    Id UUID PRIMARY KEY,
    Action VARCHAR(150) NOT NULL,
    ActorId UUID,
    OccurredAt TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    Details TEXT
);

CREATE TABLE TaxPayers (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    TaxId VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE TaxAssessments (
    Id UUID PRIMARY KEY,
    TaxPayerId UUID NOT NULL REFERENCES TaxPayers(Id),
    Amount NUMERIC(18,2) NOT NULL,
    TaxYear INT NOT NULL,
    Status VARCHAR(50) NOT NULL
);

CREATE TABLE TaxPayments (
    Id UUID PRIMARY KEY,
    TaxAssessmentId UUID NOT NULL REFERENCES TaxAssessments(Id),
    Amount NUMERIC(18,2) NOT NULL,
    PaidOn TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE VIEW TaxCollection AS
SELECT SUM(Amount) AS TotalCollected FROM TaxPayments;

CREATE TABLE SocialSecurityBenefits (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    MonthlyAmount NUMERIC(18,2) NOT NULL,
    Status VARCHAR(50) NOT NULL
);

CREATE TABLE BenefitClaims (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    BenefitType VARCHAR(100) NOT NULL,
    SubmittedAt TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP,
    Status VARCHAR(50) NOT NULL
);

CREATE TABLE BenefitPayments (
    Id UUID PRIMARY KEY,
    BenefitId UUID NOT NULL REFERENCES SocialSecurityBenefits(Id),
    Amount NUMERIC(18,2) NOT NULL,
    PaidOn TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE ContributionHistory (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    Year INT NOT NULL,
    Amount NUMERIC(18,2) NOT NULL
);

CREATE TABLE PassportApplications (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    Status VARCHAR(50) NOT NULL,
    SubmittedAt TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Passports (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    PassportNumber VARCHAR(50) NOT NULL UNIQUE,
    ExpiryDate DATE NOT NULL
);

CREATE TABLE LicenseApplications (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    Status VARCHAR(50) NOT NULL
);

CREATE TABLE DriversLicenses (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    LicenseNumber VARCHAR(50) NOT NULL UNIQUE,
    Class VARCHAR(10) NOT NULL,
    ExpiryDate DATE NOT NULL
);

CREATE TABLE VotingRegistries (
    Id UUID PRIMARY KEY,
    JurisdictionId UUID NOT NULL REFERENCES Jurisdictions(Id)
);

CREATE TABLE VoterRecords (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    VotingRegistryId UUID REFERENCES VotingRegistries(Id),
    IsActive BOOLEAN DEFAULT TRUE
);

CREATE TABLE Elections (
    Id UUID PRIMARY KEY,
    Name VARCHAR(150) NOT NULL,
    Date DATE NOT NULL,
    JurisdictionId UUID NOT NULL REFERENCES Jurisdictions(Id)
);

CREATE TABLE BallotRequests (
    Id UUID PRIMARY KEY,
    ElectionId UUID NOT NULL REFERENCES Elections(Id),
    VoterRecordId UUID NOT NULL REFERENCES VoterRecords(Id),
    Status VARCHAR(50) NOT NULL
);

CREATE TABLE ServiceRequests (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    ServiceName VARCHAR(150) NOT NULL,
    Status VARCHAR(50) NOT NULL
);

CREATE TABLE Notifications (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    Channel VARCHAR(50) NOT NULL,
    Message TEXT NOT NULL
);

CREATE TABLE DocumentVerifications (
    Id UUID PRIMARY KEY,
    CitizenId UUID NOT NULL REFERENCES Citizens(Id),
    DocumentType VARCHAR(100) NOT NULL,
    Status VARCHAR(50) NOT NULL
);

CREATE TABLE AccessPolicies (
    Id UUID PRIMARY KEY,
    Name VARCHAR(150) NOT NULL,
    Scope VARCHAR(200) NOT NULL
);

CREATE TABLE ServiceStatuses (
    Id UUID PRIMARY KEY,
    ServiceName VARCHAR(150) NOT NULL,
    Status VARCHAR(50) NOT NULL,
    CheckedAt TIMESTAMP WITH TIME ZONE DEFAULT CURRENT_TIMESTAMP
);
