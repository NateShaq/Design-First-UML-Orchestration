using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class Intersection
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IntersectionId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    public ICollection<SignalController> SignalControllers { get; set; } = new List<SignalController>();
    public ICollection<TrafficSensor> TrafficSensors { get; set; } = new List<TrafficSensor>();
}
