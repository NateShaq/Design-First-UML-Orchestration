using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml1.Api.Entities;

public enum VehicleType
{
    ElectricBus = 1,
    LightRail = 2,
    AutonomousShuttle = 3,
    Unknown = 99
}

public class VehicleDim
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VehicleId { get; set; }

    public VehicleType VehicleType { get; set; } = VehicleType.Unknown;

    public int? RouteId { get; set; }

    public RouteDim? Route { get; set; }

    // Optimistic concurrency token for ghost-write protection.
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
