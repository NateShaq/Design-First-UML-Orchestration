using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml5.Api.Domain.Entities;

public class LedgerEntry : BaseEntity
{
    public int TransactionLedgerId { get; set; }
    public TransactionLedger TransactionLedger { get; set; } = default!;

    [Required, MaxLength(16)]
    public string EntryType { get; set; } = "Debit"; // Debit or Credit

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [MaxLength(128)]
    public string Reference { get; set; } = default!;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
