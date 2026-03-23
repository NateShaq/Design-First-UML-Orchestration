using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.DTOs;

public record BookingCreateDto(
    [Required] Guid PassengerId,
    [Required] Guid PaymentAccountId
);

public record BookingCancelDto(
    [Required] VersionedDto Version
);
