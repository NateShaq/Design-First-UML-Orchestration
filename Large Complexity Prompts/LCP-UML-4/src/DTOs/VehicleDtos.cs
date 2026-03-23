using System.ComponentModel.DataAnnotations;
using FleetNetworkApi.Models;

namespace FleetNetworkApi.DTOs;

public record VehicleCreateDto(
    [Required, MaxLength(64)] string Vin,
    [Required] VehicleKind Kind,
    string? Status,
    string? Location
);

public record VehicleUpdateDto(
    [Required] VersionedDto Version,
    [Required, MaxLength(32)] string Status,
    string? Location
);
