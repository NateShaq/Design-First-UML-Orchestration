using Api.Domain;

namespace Api.Services;

public interface IGovernmentDataStore
{
    List<Citizen> Citizens { get; }
    List<IdentityRecord> Identities { get; }
    List<BiometricData> Biometrics { get; }
    List<TaxAssessment> TaxAssessments { get; }
    List<TaxPayment> TaxPayments { get; }
    List<SocialSecurityBenefits> Benefits { get; }
    List<BenefitClaim> BenefitClaims { get; }
    List<DriversLicense> Licenses { get; }
    List<LicenseApplication> LicenseApplications { get; }
    List<VoterRecord> Voters { get; }
    List<Election> Elections { get; }
    List<BallotRequest> BallotRequests { get; }
    List<AuditTrail> AuditTrails { get; }
    List<PassportApplication> PassportApplications { get; }
    List<Passport> Passports { get; }
    IdentityRegistry Registry { get; }
    TaxCollection TaxCollection { get; }
    SocialSecurityBenefits CreateBenefit(Guid citizenId, decimal amount);
    TaxAssessment CreateTaxAssessment(Guid taxpayerId, decimal amount, int year);
}

public class InMemoryGovernmentDataStore : IGovernmentDataStore
{
    public List<Citizen> Citizens { get; } = new();
    public List<IdentityRecord> Identities { get; } = new();
    public List<BiometricData> Biometrics { get; } = new();
    public List<TaxAssessment> TaxAssessments { get; } = new();
    public List<TaxPayment> TaxPayments { get; } = new();
    public List<SocialSecurityBenefits> Benefits { get; } = new();
    public List<BenefitClaim> BenefitClaims { get; } = new();
    public List<DriversLicense> Licenses { get; } = new();
    public List<LicenseApplication> LicenseApplications { get; } = new();
    public List<VoterRecord> Voters { get; } = new();
    public List<Election> Elections { get; } = new();
    public List<BallotRequest> BallotRequests { get; } = new();
    public List<AuditTrail> AuditTrails { get; } = new();
    public List<PassportApplication> PassportApplications { get; } = new();
    public List<Passport> Passports { get; } = new();

    public IdentityRegistry Registry { get; } = new();
    public TaxCollection TaxCollection { get; } = new();

    public InMemoryGovernmentDataStore()
    {
        Seed();
    }

    public SocialSecurityBenefits CreateBenefit(Guid citizenId, decimal amount)
    {
        var benefit = new SocialSecurityBenefits
        {
            CitizenId = citizenId,
            MonthlyAmount = amount
        };
        Benefits.Add(benefit);
        return benefit;
    }

    public TaxAssessment CreateTaxAssessment(Guid taxpayerId, decimal amount, int year)
    {
        var assessment = new TaxAssessment
        {
            TaxPayerId = taxpayerId,
            Amount = amount,
            TaxYear = year
        };
        TaxAssessments.Add(assessment);
        return assessment;
    }

    private void Seed()
    {
        var citizen = new Citizen
        {
            NationalId = "USA-0001",
            FullName = "Ada Lovelace",
            DateOfBirth = new DateOnly(1815, 12, 10),
            Email = "ada@example.gov",
            Phone = "+1-555-0001"
        };

        var voter = new VoterRecord { CitizenId = citizen.Id, IsActive = true };

        Citizens.Add(citizen);
        Identities.Add(new IdentityRecord { CitizenId = citizen.Id });
        Biometrics.Add(new BiometricData { CitizenId = citizen.Id, Modality = "Fingerprint", Hash = "hash-ada" });
        BenefitClaims.Add(new BenefitClaim { CitizenId = citizen.Id, BenefitType = "Retirement" });
        Benefits.Add(new SocialSecurityBenefits { CitizenId = citizen.Id, MonthlyAmount = 1200m });
        LicenseApplications.Add(new LicenseApplication { CitizenId = citizen.Id, Status = "Approved" });
        Licenses.Add(new DriversLicense
        {
            CitizenId = citizen.Id,
            LicenseNumber = "D-1001",
            Class = "C",
            ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(6))
        });
        PassportApplications.Add(new PassportApplication { CitizenId = citizen.Id, Status = "Approved" });
        Passports.Add(new Passport
        {
            CitizenId = citizen.Id,
            PassportNumber = "P-0001",
            ExpiryDate = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(10))
        });
        Elections.Add(new Election
        {
            Name = "General Election",
            Date = new DateOnly(DateTime.UtcNow.Year, 11, 5),
            JurisdictionId = Guid.NewGuid()
        });
        Voters.Add(voter);
        BallotRequests.Add(new BallotRequest { ElectionId = Elections.First().Id, VoterRecordId = voter.Id });
        Registry.Records.Add(new IdentityRecord { CitizenId = citizen.Id, Status = "Verified" });
        TaxCollection.TotalCollected = 0m;
        AuditTrails.Add(new AuditTrail { ActorId = citizen.Id, Action = "SeedData", Details = "Initial load" });
    }
}
