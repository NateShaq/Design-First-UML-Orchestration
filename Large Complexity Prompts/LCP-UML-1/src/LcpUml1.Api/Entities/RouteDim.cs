using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class RouteDim
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RouteId { get; set; }

    [Required]
    [MaxLength(64)]
    public string Mode { get; set; } = "Unknown";

    public ICollection<VehicleDim> Vehicles { get; set; } = new List<VehicleDim>();
}
