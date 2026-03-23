using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml5.Api.Domain.Entities;

public class SavingsAccount : Account
{
    [Column(TypeName = "decimal(5,3)")]
    public decimal InterestRate { get; set; }

    // Logical version for optimistic control (separate from RowVersion).
    public long Version { get; set; } = 1;

    [MaxLength(64)]
    public string? IdempotencyKey { get; set; }
}
