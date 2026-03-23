using System.ComponentModel.DataAnnotations;

namespace LcpUml5.Api.Domain.Entities;

public class StockPortfolio : Portfolio
{
    public long Version { get; set; } = 1;

    [MaxLength(64)]
    public string? IdempotencyKey { get; set; }

    public ICollection<StockHolding> Holdings { get; set; } = new List<StockHolding>();
}
