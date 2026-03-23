using System.ComponentModel.DataAnnotations;

namespace LcpUml5.Api.Domain.Entities;

public class CryptoWallet : Account
{
    [Required, MaxLength(32)]
    public string Chain { get; set; } = default!;

    [Required, MaxLength(128)]
    public string PublicAddress { get; set; } = default!;

    public long Version { get; set; } = 1;

    [MaxLength(64)]
    public string? IdempotencyKey { get; set; }
}
