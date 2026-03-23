using System;
using System.ComponentModel.DataAnnotations;

namespace OmnichannelRetail.Api.Models;

public class Product
{
    [Key]
    [MaxLength(50)]
    public string Sku { get; set; } = string.Empty;

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    public double Weight { get; set; }

    public string Discriminator { get; set; } = "Standard";

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class PerishableGoods : Product
{
    public DateOnly? ExpirationDate { get; set; }
    [MaxLength(100)]
    public string? TemperatureRange { get; set; }
    public long Version { get; set; }
}

public class LuxuryItems : Product
{
    [MaxLength(200)]
    public string? AuthCertificate { get; set; }
    public decimal InsuranceValue { get; set; }
    public long Version { get; set; }
}

public class DigitalDownloads : Product
{
    public double FileSizeMB { get; set; }
    [MaxLength(500)]
    public string? DownloadUrl { get; set; }
    public long Version { get; set; }
}
