using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class GridPowerMonitor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GridPowerMonitorId { get; set; }

    [Required]
    [MaxLength(128)]
    public string Location { get; set; } = string.Empty;

    [Column(TypeName = "decimal(10,2)")]
    public decimal CurrentLoadKw { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public ICollection<PowerEventFact> PowerEvents { get; set; } = new List<PowerEventFact>();
}
