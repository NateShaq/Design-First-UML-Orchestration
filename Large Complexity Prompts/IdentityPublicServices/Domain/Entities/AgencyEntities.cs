using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityPublicServices.Domain.Entities;

public abstract class Agency
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(160)]
    public string AgencyName { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? ContactPoint { get; set; }
}

public class MunicipalService : Agency
{
    [Required, MaxLength(120)]
    public string City { get; set; } = string.Empty;

    [Required, MaxLength(60)]
    public string ServiceTier { get; set; } = string.Empty;

    public ICollection<DriversLicense> DriversLicenses { get; set; } = new List<DriversLicense>();
    public ICollection<VotingRegistry> VotingRegistries { get; set; } = new List<VotingRegistry>();
}

public class NationalService : Agency
{
    [Required, MaxLength(120)]
    public string Country { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string OversightBody { get; set; } = string.Empty;

    public ICollection<TaxCollection> TaxCollections { get; set; } = new List<TaxCollection>();
    public ICollection<SocialSecurityBenefits> SocialSecurityBenefits { get; set; } = new List<SocialSecurityBenefits>();
    public ICollection<HealthServices> HealthServices { get; set; } = new List<HealthServices>();
    public ICollection<EducationServices> EducationServices { get; set; } = new List<EducationServices>();
    public ICollection<PassportService> PassportServices { get; set; } = new List<PassportService>();
}

public class TaxCollection
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(40)]
    public string AgencyCode { get; set; } = string.Empty;

    public int FiscalYear { get; set; }

    public Guid NationalServiceId { get; set; }
    public NationalService NationalService { get; set; } = null!;

    public Guid? PublicRecordId { get; set; }
    public PublicRecord? PublicRecord { get; set; }
}

public class SocialSecurityBenefits
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(80)]
    public string ProgramId { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string BenefitType { get; set; } = string.Empty;

    public Guid NationalServiceId { get; set; }
    public NationalService NationalService { get; set; } = null!;

    public Guid? PublicRecordId { get; set; }
    public PublicRecord? PublicRecord { get; set; }
}

public class PassportService
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(120)]
    public string PassportOffice { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal IssuanceFee { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public Guid NationalServiceId { get; set; }
    public NationalService NationalService { get; set; } = null!;

    public Guid? CredentialId { get; set; }
    public Credential? Credential { get; set; }
}

public class DriversLicense
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(20)]
    public string LicenseClass { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string IssuedState { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public Guid MunicipalServiceId { get; set; }
    public MunicipalService MunicipalService { get; set; } = null!;

    public Guid CredentialId { get; set; }
    public Credential Credential { get; set; } = null!;
}

public class VotingRegistry
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(80)]
    public string District { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string EligibilityRule { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public Guid MunicipalServiceId { get; set; }
    public MunicipalService MunicipalService { get; set; } = null!;

    public Guid PublicRecordId { get; set; }
    public PublicRecord PublicRecord { get; set; } = null!;
}

public class HealthServices
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(120)]
    public string ProviderNetwork { get; set; } = string.Empty;

    [Required, MaxLength(160)]
    public string CoverageScope { get; set; } = string.Empty;

    public Guid NationalServiceId { get; set; }
    public NationalService NationalService { get; set; } = null!;
}

public class EducationServices
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(80)]
    public string EducationLevel { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string InstitutionType { get; set; } = string.Empty;

    public Guid NationalServiceId { get; set; }
    public NationalService NationalService { get; set; } = null!;
}
