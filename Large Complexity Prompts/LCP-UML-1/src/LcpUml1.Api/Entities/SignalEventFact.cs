using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class SignalEventFact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EventId { get; set; }

    public int SignalControllerId { get; set; }
    public SignalController? SignalController { get; set; }

    public int DateId { get; set; }
    public CalendarDim? Calendar { get; set; }

    public DateTime Timestamp { get; set; }

    [MaxLength(32)]
    public string State { get; set; } = string.Empty;
}
