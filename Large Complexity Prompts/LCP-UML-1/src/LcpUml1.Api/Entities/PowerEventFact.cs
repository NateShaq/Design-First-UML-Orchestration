using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class PowerEventFact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EventId { get; set; }

    public int GridPowerMonitorId { get; set; }
    public GridPowerMonitor? GridPowerMonitor { get; set; }

    public int DateId { get; set; }
    public CalendarDim? Calendar { get; set; }

    public DateTime Timestamp { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal LoadKw { get; set; }
}
