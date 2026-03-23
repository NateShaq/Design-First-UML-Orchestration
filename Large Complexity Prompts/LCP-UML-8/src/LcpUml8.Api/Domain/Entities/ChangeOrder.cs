using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml8.Api.Domain.Entities;

public class ChangeOrder
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public long Version { get; set; }

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Draft";

    [Required, MaxLength(50)]
    public string Priority { get; set; } = "Normal";

    [Timestamp]
    [Column(TypeName = "rowversion")]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public string? ProductId { get; set; }
    public Product? Product { get; set; }

    public string? EngineeringDesignId { get; set; }
    public EngineeringDesign? EngineeringDesign { get; set; }

    public string? WorkOrderId { get; set; }
    public WorkOrder? WorkOrder { get; set; }

    public string? InspectionPlanId { get; set; }
    public InspectionPlan? InspectionPlan { get; set; }
}
