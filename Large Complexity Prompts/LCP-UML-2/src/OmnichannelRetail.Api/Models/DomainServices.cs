using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmnichannelRetail.Api.Models;

public class EcommercePlatform
{
    [Key]
    public Guid EcommercePlatformId { get; set; }

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    public string SupportedChannels { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class POSSystem
{
    [Key]
    [MaxLength(50)]
    public string TerminalId { get; set; } = string.Empty;

    public Guid PaymentGatewayId { get; set; }
    public PaymentGateway? PaymentGateway { get; set; }

    public Guid TaxCalculatorId { get; set; }
    public TaxCalculator? TaxCalculator { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class SupplyChainLogistics
{
    [Key]
    public Guid SupplyChainLogisticsId { get; set; }

    public Guid WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class TaxCalculator
{
    [Key]
    public Guid TaxCalculatorId { get; set; }

    [MaxLength(100)]
    public string Jurisdiction { get; set; } = string.Empty;

    public long Version { get; set; }

    public Guid? LockToken { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class AnalyticsService
{
    [Key]
    public Guid AnalyticsServiceId { get; set; }

    [MaxLength(200)]
    public string? Endpoint { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class NotificationService
{
    [Key]
    public Guid NotificationServiceId { get; set; }

    [MaxLength(50)]
    public string Channel { get; set; } = string.Empty;

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
