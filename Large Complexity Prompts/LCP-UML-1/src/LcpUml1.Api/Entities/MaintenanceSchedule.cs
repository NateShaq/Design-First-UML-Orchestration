using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class MaintenanceSchedule
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaintenanceScheduleId { get; set; }

    public int VehicleId { get; set; }
    public VehicleDim? Vehicle { get; set; }

    public DateOnly? NextServiceDate { get; set; }

    [MaxLength(32)]
    public string Status { get; set; } = "Planned";

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public ICollection<MaintenanceFact> MaintenanceFacts { get; set; } = new List<MaintenanceFact>();
}
