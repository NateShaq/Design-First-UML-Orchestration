using System.ComponentModel.DataAnnotations;

namespace LcpUml5.Api.Domain.Entities;

public class CustomerProfile : BaseEntity
{
    [Required, MaxLength(64)]
    public string CustomerId { get; set; } = default!;

    [MaxLength(32)]
    public string Segment { get; set; } = "Retail";

    [MaxLength(32)]
    public string RiskLevel { get; set; } = "Moderate";

    public ICollection<Account> Accounts { get; set; } = new List<Account>();
    public ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    public ICollection<RiskAssessment> RiskAssessments { get; set; } = new List<RiskAssessment>();
    public ICollection<TaxDocument> TaxDocuments { get; set; } = new List<TaxDocument>();

    public KycVerification? KycVerification { get; set; }
}
