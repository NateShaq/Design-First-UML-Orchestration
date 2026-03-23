using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.Models;

public class PassengerProfile : BaseEntity
{
    [Required, MaxLength(128)]
    public string Name { get; set; } = string.Empty;

    [EmailAddress]
    public string? Email { get; set; }
}
