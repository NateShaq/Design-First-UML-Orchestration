using System;

namespace LcpVibe5.Domain;

public class RiskAssessment
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public int Score { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime AssessedOn { get; set; }
}

public class KYCVerification
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Status { get; set; } = "Pending";
    public DateTime? VerifiedOn { get; set; }
}

public class AMLAlert
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Severity { get; set; } = "Medium";
    public string RuleId { get; set; } = string.Empty;
    public DateTime RaisedOn { get; set; }
    public string Status { get; set; } = "Open";
}

public class TransactionMonitoringRule
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Expression { get; set; } = string.Empty;
    public string Severity { get; set; } = "Medium";
    public bool Enabled { get; set; }
}

public class AuditTrail
{
    public Guid Id { get; set; }
    public Guid ActorId { get; set; }
    public string Action { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public string Details { get; set; } = string.Empty;
}

public class TaxDocument
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Type { get; set; } = string.Empty; // e.g., 1099, 1042-S
    public int TaxYear { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public DateTime GeneratedOn { get; set; }
}

public class RegulatoryReport
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public string Status { get; set; } = "Draft";
}

public class SanctionListEntry
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ListName { get; set; } = string.Empty;
    public DateTime ListedOn { get; set; }
    public bool IsActive { get; set; }
}

public class ConsentRecord
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string ConsentType { get; set; } = string.Empty;
    public DateTime GrantedOn { get; set; }
    public DateTime? RevokedOn { get; set; }
}

public class DataRetentionPolicy
{
    public Guid Id { get; set; }
    public string DataCategory { get; set; } = string.Empty;
    public int RetentionMonths { get; set; }
    public bool AutoPurgeEnabled { get; set; }
}
