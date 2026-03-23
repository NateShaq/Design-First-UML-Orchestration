using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class TrafficSensor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TrafficSensorId { get; set; }

    [Required]
    [MaxLength(64)]
    public string SensorType { get; set; } = "Loop";

    public int? IntersectionId { get; set; }
    public Intersection? Intersection { get; set; }

    [MaxLength(32)]
    public string Status { get; set; } = "Active";

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
