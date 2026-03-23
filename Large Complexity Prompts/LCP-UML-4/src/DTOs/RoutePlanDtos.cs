using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.DTOs;

public record RoutePlanCreateDto(
    [Required] Guid VehicleId,
    [MaxLength(128)] string MapRef,
    [MaxLength(128)] string TrafficRef,
    string? LockToken
);

public record RoutePlanCommitDto(
    [Required] VersionedDto Version,
    [MaxLength(128)] string? MapRef,
    [MaxLength(128)] string? TrafficRef
);
