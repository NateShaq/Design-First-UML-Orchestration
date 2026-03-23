using System.ComponentModel.DataAnnotations;

namespace LcpUml5.Api.Domain.Entities;

public class KycVerification : BaseEntity
{
    [MaxLength(32)]
    public string Status { get; set; } = "Pending";

    public long Version { get; set; } = 1;

    [MaxLength(64)]
    public string? IdempotencyKey { get; set; }

    public int CustomerProfileId { get; set; }
    public CustomerProfile CustomerProfile { get; set; } = default!;
}
