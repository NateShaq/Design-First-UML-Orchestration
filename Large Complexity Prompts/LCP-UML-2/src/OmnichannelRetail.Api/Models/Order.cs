using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmnichannelRetail.Api.Models;

public class Order
{
    [Key]
    [MaxLength(50)]
    public string OrderNumber { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Status { get; set; } = "Pending";

    [Range(0, double.MaxValue)]
    public decimal Total { get; set; }

    public Guid? TransactionId { get; set; }

    [MaxLength(30)]
    public string? IsolationLevel { get; set; }

    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public ICollection<OrderLine> Lines { get; set; } = new List<OrderLine>();

    public Guid? PaymentTransactionId { get; set; }
    public PaymentTransaction? PaymentTransaction { get; set; }

    public Guid? ShipmentId { get; set; }
    public Shipment? Shipment { get; set; }

    public Guid? ReturnAuthorizationId { get; set; }
    public ReturnAuthorization? ReturnAuthorization { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class OrderLine
{
    [Key]
    public Guid OrderLineId { get; set; }

    [Required]
    public string OrderNumber { get; set; } = string.Empty;
    public Order? Order { get; set; }

    [Required, MaxLength(50)]
    public string ProductSku { get; set; } = string.Empty;
    public Product? Product { get; set; }

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
