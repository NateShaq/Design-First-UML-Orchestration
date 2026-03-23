using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.DTOs;

public record TripAssignDto(
    [Required] Guid BookingId,
    [Required] Guid VehicleId
);

public record TripCommitDto(
    [Required] VersionedDto Version,
    [Required, MaxLength(32)] string Status
);
