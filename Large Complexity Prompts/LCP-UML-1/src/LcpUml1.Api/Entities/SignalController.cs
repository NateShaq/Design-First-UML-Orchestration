using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class SignalController
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int SignalControllerId { get; set; }

    [Required]
    [MaxLength(128)]
    public string ControllerName { get; set; } = string.Empty;

    public int? IntersectionId { get; set; }
    public Intersection? Intersection { get; set; }

    [MaxLength(32)]
    public string State { get; set; } = "Unknown";

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public ICollection<SignalEventFact> Events { get; set; } = new List<SignalEventFact>();
}
