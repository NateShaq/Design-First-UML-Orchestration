using System.ComponentModel.DataAnnotations;

namespace LcpUml5.Api.Domain.Entities;

public class TransactionLedger : BaseEntity
{
    [Required, MaxLength(64)]
    public string LedgerId { get; set; } = default!;

    public long Version { get; set; } = 1;

    [MaxLength(64)]
    public string? IdempotencyKey { get; set; }

    public ICollection<LedgerEntry> Entries { get; set; } = new List<LedgerEntry>();
}
