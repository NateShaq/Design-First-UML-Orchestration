using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmnichannelRetail.Api.Models;

public class ShoppingCart
{
    [Key]
    public Guid CartId { get; set; }

    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class CartItem
{
    [Key]
    public Guid CartItemId { get; set; }

    [Required]
    public Guid CartId { get; set; }
    public ShoppingCart? Cart { get; set; }

    [Required, MaxLength(50)]
    public string ProductSku { get; set; } = string.Empty;
    public Product? Product { get; set; }

    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }
}
