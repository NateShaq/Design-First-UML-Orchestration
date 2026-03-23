using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.DTOs;

public record IncidentCreateDto(
    [Required] Guid VehicleId,
    Guid? RoutePlanId,
    [Required, MaxLength(512)] string Summary,
    string? EvidenceUri
);

public record IncidentAppendEvidenceDto(
    [Required] VersionedDto Version,
    [Required, MaxLength(512)] string EvidenceUri
);
