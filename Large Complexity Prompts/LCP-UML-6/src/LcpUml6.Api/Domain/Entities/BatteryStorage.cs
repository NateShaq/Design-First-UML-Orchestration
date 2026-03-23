using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class BatteryStorage : ThreadSafeEntity
{
    [Key]
    public Guid AssetId { get; set; }

    public decimal CapacityMwh { get; set; }

    [MaxLength(100)]
    public string Chemistry { get; set; } = "Li-ion";

    [MaxLength(4000)]
    public string? Notes { get; set; }
}
