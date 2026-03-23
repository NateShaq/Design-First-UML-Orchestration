using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml5.Api.Domain.Entities;

public abstract class Account : BaseEntity
{
    [Required, MaxLength(32)]
    public string AccountNumber { get; set; } = default!;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }

    [Required, MaxLength(8)]
    public string Currency { get; set; } = "USD";

    public int CustomerProfileId { get; set; }
    public CustomerProfile CustomerProfile { get; set; } = default!;
}
