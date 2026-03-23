using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml5.Api.Domain.Entities;

public class StockHolding : BaseEntity
{
    public int StockPortfolioId { get; set; }
    public StockPortfolio StockPortfolio { get; set; } = default!;

    [Required, MaxLength(16)]
    public string Symbol { get; set; } = default!;

    [Column(TypeName = "decimal(18,4)")]
    public decimal Quantity { get; set; }

    [Column(TypeName = "decimal(18,4)")]
    public decimal CostBasis { get; set; }
}
