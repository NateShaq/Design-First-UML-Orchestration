namespace Api.Domain;

public class Citizen
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string NationalId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
}

public class IdentityRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Verified";
}

public class IdentityRegistry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string RegistryName { get; set; } = "National Identity Registry";
    public List<IdentityRecord> Records { get; set; } = new();
}

public class BiometricData
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string Modality { get; set; } = string.Empty; // e.g., fingerprint, face
    public string Hash { get; set; } = string.Empty;
    public DateTime CapturedAt { get; set; } = DateTime.UtcNow;
}

public class PublicRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string RecordType { get; set; } = string.Empty;
    public string Data { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public class Jurisdiction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string RegionCode { get; set; } = string.Empty;
}

public class AuditTrail
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Action { get; set; } = string.Empty;
    public Guid? ActorId { get; set; }
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
    public string Details { get; set; } = string.Empty;
}

public class TaxPayer
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string TaxId { get; set; } = string.Empty;
}

public class TaxAssessment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TaxPayerId { get; set; }
    public decimal Amount { get; set; }
    public int TaxYear { get; set; }
    public string Status { get; set; } = "Open";
}

public class TaxPayment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid TaxAssessmentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidOn { get; set; } = DateTime.UtcNow;
}

public class TaxCollection
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public decimal TotalCollected { get; set; }
    public List<TaxPayment> Payments { get; set; } = new();
}

public class SocialSecurityBenefits
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public decimal MonthlyAmount { get; set; }
    public string Status { get; set; } = "Active";
}

public class BenefitClaim
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string BenefitType { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
}

public class BenefitPayment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BenefitId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidOn { get; set; } = DateTime.UtcNow;
}

public class ContributionHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public int Year { get; set; }
    public decimal Amount { get; set; }
}

public class PassportService
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public List<PassportApplication> Applications { get; set; } = new();
}

public class PassportApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string Status { get; set; } = "Submitted";
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
}

public class Passport
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public DateOnly ExpiryDate { get; set; }
}

public class DriversLicense
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public string Class { get; set; } = "C";
    public DateOnly ExpiryDate { get; set; }
}

public class LicenseApplication
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string Status { get; set; } = "Pending";
}

public class License
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LicenseApplicationId { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
}

public class VotingRegistry
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid JurisdictionId { get; set; }
    public List<VoterRecord> Voters { get; set; } = new();
}

public class VoterRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class Election
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public Guid JurisdictionId { get; set; }
}

public class BallotRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ElectionId { get; set; }
    public Guid VoterRecordId { get; set; }
    public string Status { get; set; } = "Requested";
}

public class ServiceRequest
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string Status { get; set; } = "Open";
}

public class Notification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string Channel { get; set; } = "Email";
    public string Message { get; set; } = string.Empty;
}

public class DocumentVerification
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid CitizenId { get; set; }
    public string DocumentType { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
}

public class AccessPolicy
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
}

public class ServiceStatus
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string ServiceName { get; set; } = string.Empty;
    public string Status { get; set; } = "Operational";
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
}
