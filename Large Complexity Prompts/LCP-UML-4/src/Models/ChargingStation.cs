using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.Models;

public class ChargingStation : BaseEntity
{
    [MaxLength(128)]
    public string Location { get; set; } = string.Empty;

    [MaxLength(32)]
    public string SlotStatus { get; set; } = "available";

    public Guid? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
}
