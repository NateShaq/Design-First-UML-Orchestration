using System.ComponentModel.DataAnnotations;

namespace LcpUml8.Api.Domain.Entities;

public class QualityAssurance
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public ICollection<InspectionPlan> InspectionPlans { get; set; } = new List<InspectionPlan>();
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

public class InspectionPlan
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(200)]
    public string Method { get; set; } = string.Empty;

    public ICollection<InspectionResult> InspectionResults { get; set; } = new List<InspectionResult>();
    public ICollection<ChangeOrder> ChangeOrders { get; set; } = new List<ChangeOrder>();

    [Required]
    public string QualityAssuranceId { get; set; } = string.Empty;
    public QualityAssurance? QualityAssurance { get; set; }
}

public class InspectionResult
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [Required]
    public string InspectionPlanId { get; set; } = string.Empty;
    public InspectionPlan? InspectionPlan { get; set; }

    public ICollection<NonConformance> NonConformances { get; set; } = new List<NonConformance>();
    public ICollection<TraceabilityRecord> TraceabilityRecords { get; set; } = new List<TraceabilityRecord>();
}

public class NonConformance
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(50)]
    public string Severity { get; set; } = "Minor";

    [Required]
    public string InspectionResultId { get; set; } = string.Empty;
    public InspectionResult? InspectionResult { get; set; }

    public ICollection<CorrectiveAction> CorrectiveActions { get; set; } = new List<CorrectiveAction>();
}

public class CorrectiveAction
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(200)]
    public string Action { get; set; } = string.Empty;

    [Required]
    public string NonConformanceId { get; set; } = string.Empty;
    public NonConformance? NonConformance { get; set; }
}
