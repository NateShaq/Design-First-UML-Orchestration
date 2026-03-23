using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public class MaintenanceFact
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaintenanceId { get; set; }

    public int VehicleId { get; set; }
    public VehicleDim? Vehicle { get; set; }

    public int ScheduleId { get; set; }
    public MaintenanceSchedule? Schedule { get; set; }

    public int DateId { get; set; }
    public CalendarDim? Calendar { get; set; }

    [MaxLength(32)]
    public string Status { get; set; } = "Planned";
}
