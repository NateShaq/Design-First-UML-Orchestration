using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class MaintenanceSchedule
{
    [Key]
    public Guid MaintenanceScheduleId { get; set; }

    public DateTime NextServiceDate { get; set; }

    [MaxLength(4000)]
    public string? Notes { get; set; }
}
