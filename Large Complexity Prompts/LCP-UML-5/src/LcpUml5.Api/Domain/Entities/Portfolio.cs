using System.ComponentModel.DataAnnotations;

namespace LcpUml5.Api.Domain.Entities;

public abstract class Portfolio : BaseEntity
{
    [Required, MaxLength(64)]
    public string PortfolioCode { get; set; } = default!;

    [MaxLength(128)]
    public string Strategy { get; set; } = "Balanced";

    public int CustomerProfileId { get; set; }
    public CustomerProfile CustomerProfile { get; set; } = default!;
}
