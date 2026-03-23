using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml6.Api.Domain.Entities;

public class SolarFarm : ThreadSafeEntity
{
    [Key]
    public Guid AssetId { get; set; }

    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public Guid? GridConnectionId { get; set; }
    public GridConnection? GridConnection { get; set; }

    public Guid? WeatherStationId { get; set; }
    public WeatherStation? WeatherStation { get; set; }

    public Guid? BatteryStorageId { get; set; }
    public BatteryStorage? BatteryStorage { get; set; }

    public Guid? InverterId { get; set; }
    public Inverter? Inverter { get; set; }

    public Guid? MaintenanceScheduleId { get; set; }
    public MaintenanceSchedule? MaintenanceSchedule { get; set; }

    [MaxLength(4000)]
    public string? Notes { get; set; }
}
