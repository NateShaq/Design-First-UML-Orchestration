using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.DTOs;

public record SlotReserveDto(
    [Required] Guid VehicleId,
    [Required] VersionedDto Version
);
