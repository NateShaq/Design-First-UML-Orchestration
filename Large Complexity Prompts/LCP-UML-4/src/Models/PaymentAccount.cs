using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.Models;

public class PaymentAccount : BaseEntity
{
    [Required, MaxLength(4)]
    public string Last4 { get; set; } = string.Empty;

    [MaxLength(16)]
    public string Brand { get; set; } = string.Empty;
}
