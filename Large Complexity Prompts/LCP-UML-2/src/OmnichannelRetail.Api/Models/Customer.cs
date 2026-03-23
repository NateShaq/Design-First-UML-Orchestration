using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmnichannelRetail.Api.Models;

public class Customer
{
    [Key]
    public Guid CustomerId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(320)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Segment { get; set; }

    public ICollection<ShoppingCart> Carts { get; set; } = new List<ShoppingCart>();
    public LoyaltyAccount? LoyaltyAccount { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
