using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.DTOs;

public record MaintenanceCreateDto(
    [Required] Guid VehicleId,
    [Required] DateTimeOffset ScheduledAt,
    [MaxLength(256)] string Description
);

public record MaintenanceUpdateDto(
    [Required] VersionedDto Version,
    [Required, MaxLength(32)] string Status
);
