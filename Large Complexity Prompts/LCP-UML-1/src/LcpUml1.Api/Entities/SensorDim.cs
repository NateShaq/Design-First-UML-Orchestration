using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class SensorDim
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SensorId { get; set; }

    public int? IntersectionId { get; set; }
    public Intersection? Intersection { get; set; }
}
