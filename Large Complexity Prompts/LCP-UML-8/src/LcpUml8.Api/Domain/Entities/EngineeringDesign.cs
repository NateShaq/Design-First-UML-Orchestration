using System.ComponentModel.DataAnnotations;

namespace LcpUml8.Api.Domain.Entities;

public class EngineeringDesign
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(50)]
    public string LifecycleState { get; set; } = "Draft";

    public ICollection<CADModel> CadModels { get; set; } = new List<CADModel>();
    public ICollection<Drawing> Drawings { get; set; } = new List<Drawing>();
    public ICollection<Revision> Revisions { get; set; } = new List<Revision>();
    public ICollection<Engineer> Owners { get; set; } = new List<Engineer>();
    public ICollection<ChangeOrder> ChangeOrders { get; set; } = new List<ChangeOrder>();
}

public class CADModel
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(500)]
    public string FilePath { get; set; } = string.Empty;

    [Required]
    public string EngineeringDesignId { get; set; } = string.Empty;
    public EngineeringDesign? EngineeringDesign { get; set; }
}

public class Drawing
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(500)]
    public string PdfPath { get; set; } = string.Empty;

    [Required]
    public string EngineeringDesignId { get; set; } = string.Empty;
    public EngineeringDesign? EngineeringDesign { get; set; }
}

public class Revision
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(20)]
    public string RevCode { get; set; } = string.Empty;

    [Required]
    public string EngineeringDesignId { get; set; } = string.Empty;
    public EngineeringDesign? EngineeringDesign { get; set; }
}

public class Engineer
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(50)]
    public string Role { get; set; } = string.Empty;

    public ICollection<EngineeringDesign> EngineeringDesigns { get; set; } = new List<EngineeringDesign>();
}
