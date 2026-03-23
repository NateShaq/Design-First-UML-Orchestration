using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class HydroPlant : ThreadSafeEntity
{
    [Key]
    public Guid AssetId { get; set; }

    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public Guid? GridConnectionId { get; set; }
    public GridConnection? GridConnection { get; set; }

    public Guid? MaintenanceScheduleId { get; set; }
    public MaintenanceSchedule? MaintenanceSchedule { get; set; }

    [MaxLength(4000)]
    public string? Notes { get; set; }
}
