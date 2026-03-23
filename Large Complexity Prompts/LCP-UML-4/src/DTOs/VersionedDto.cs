using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.DTOs;

public record VersionedDto(
    [Required] Guid Id,
    [Required] string RowVersionBase64,
    string? LockToken
)
{
    public byte[] RowVersionBytes => Convert.FromBase64String(RowVersionBase64);
}
