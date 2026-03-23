using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityPublicServices.Domain.Entities;

public class IdentityRegistry
{
    [Key]
    public Guid RegistryId { get; set; }

    [MaxLength(64)]
    public string Status { get; set; } = "Active";

    public Guid PersonId { get; set; }
    public Person Person { get; set; } = null!;

    public Guid? JurisdictionId { get; set; }
    public Jurisdiction? Jurisdiction { get; set; }

    public Guid? IdentityProviderId { get; set; }
    public IdentityProvider? IdentityProvider { get; set; }

    public ICollection<BiometricData> BiometricData { get; set; } = new List<BiometricData>();
    public ICollection<PublicRecord> PublicRecords { get; set; } = new List<PublicRecord>();
    public ICollection<AuditTrail> AuditTrails { get; set; } = new List<AuditTrail>();
}

public class Person
{
    [Key]
    public Guid PersonId { get; set; }

    [Required, MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    public ICollection<IdentityRegistry> IdentityRegistries { get; set; } = new List<IdentityRegistry>();
}

public class Citizen : Person
{
    [Required, MaxLength(80)]
    public string CitizenshipStatus { get; set; } = string.Empty;
}

public class Resident : Person
{
    [Required, MaxLength(80)]
    public string ResidencyPermit { get; set; } = string.Empty;
}

public class ForeignNational : Person
{
    [Required, MaxLength(80)]
    public string VisaType { get; set; } = string.Empty;
}

public class BiometricData
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(256)]
    public string FingerprintHash { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string FaceTemplate { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public Guid IdentityRegistryId { get; set; }
    public IdentityRegistry IdentityRegistry { get; set; } = null!;

    public Guid? ConsentRecordId { get; set; }
    public ConsentRecord? ConsentRecord { get; set; }
}

public class AuditTrail
{
    [Key]
    public Guid EventId { get; set; }

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public Guid TxId { get; set; }

    [Required, MaxLength(32)]
    public string IsolationLevel { get; set; } = "Serializable";

    [Required, MaxLength(32)]
    public string CommitState { get; set; } = "Pending";

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public Guid IdentityRegistryId { get; set; }
    public IdentityRegistry IdentityRegistry { get; set; } = null!;

    public Guid? DataRetentionPolicyId { get; set; }
    public DataRetentionPolicy? DataRetentionPolicy { get; set; }
}

public class Jurisdiction
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(60)]
    public string Level { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public ICollection<IdentityRegistry> IdentityRegistries { get; set; } = new List<IdentityRegistry>();
    public ICollection<PublicRecord> PublicRecords { get; set; } = new List<PublicRecord>();
}

public class PublicRecord
{
    [Key]
    public Guid RecordId { get; set; }

    [Required, MaxLength(100)]
    public string RecordType { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public Guid IdentityRegistryId { get; set; }
    public IdentityRegistry IdentityRegistry { get; set; } = null!;

    public Guid JurisdictionId { get; set; }
    public Jurisdiction Jurisdiction { get; set; } = null!;
}

public class DataRetentionPolicy
{
    [Key]
    public Guid Id { get; set; }

    public int RetentionYears { get; set; }

    [Required, MaxLength(120)]
    public string ArchivalMethod { get; set; } = string.Empty;

    public ICollection<AuditTrail> AuditTrails { get; set; } = new List<AuditTrail>();
}

public class ConsentRecord
{
    [Key]
    public Guid ConsentId { get; set; }

    [Required, MaxLength(120)]
    public string Scope { get; set; } = string.Empty;

    public ICollection<BiometricData> BiometricData { get; set; } = new List<BiometricData>();
}
