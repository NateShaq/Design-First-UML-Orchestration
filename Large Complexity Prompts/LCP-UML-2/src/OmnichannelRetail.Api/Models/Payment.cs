using System;
using System.ComponentModel.DataAnnotations;

namespace OmnichannelRetail.Api.Models;

public class PaymentGateway
{
    [Key]
    public Guid PaymentGatewayId { get; set; }

    [MaxLength(100)]
    public string Provider { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class PaymentTransaction
{
    [Key]
    public Guid PaymentTransactionId { get; set; }

    public Guid PaymentGatewayId { get; set; }
    public PaymentGateway? PaymentGateway { get; set; }

    public string? OrderNumber { get; set; }
    public Order? Order { get; set; }

    public decimal Amount { get; set; }

    [MaxLength(30)]
    public string Status { get; set; } = "Initiated";

    public Guid? TransactionId { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class RefundProcessor
{
    [Key]
    public Guid RefundId { get; set; }

    [MaxLength(50)]
    public string Method { get; set; } = string.Empty;

    public Guid PaymentGatewayId { get; set; }
    public PaymentGateway? PaymentGateway { get; set; }

    public decimal Amount { get; set; }

    public Guid? TransactionId { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class ReturnAuthorization
{
    [Key]
    [MaxLength(50)]
    public string RmaNumber { get; set; } = string.Empty;

    [MaxLength(200)]
    public string? Reason { get; set; }

    [MaxLength(30)]
    public string Status { get; set; } = "Pending";

    public long Version { get; set; }

    public Guid? RefundId { get; set; }
    public RefundProcessor? RefundProcessor { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
