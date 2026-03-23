using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml5.Api.Domain.Entities;

public class RiskAssessment : BaseEntity
{
    [Column(TypeName = "decimal(5,2)")]
    public decimal Score { get; set; }

    public long Version { get; set; } = 1;

    [MaxLength(64)]
    public string? IdempotencyKey { get; set; }

    public int CustomerProfileId { get; set; }
    public CustomerProfile CustomerProfile { get; set; } = default!;
}
