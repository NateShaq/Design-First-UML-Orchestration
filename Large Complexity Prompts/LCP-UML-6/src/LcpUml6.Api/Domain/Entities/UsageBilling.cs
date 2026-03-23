using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class UsageBilling : ThreadSafeEntity
{
    [Key]
    public Guid BillingId { get; set; }

    public Guid MeterId { get; set; }
    public SmartMeter? Meter { get; set; }

    public Guid TariffPlanId { get; set; }
    public TariffPlan? TariffPlan { get; set; }

    public Guid SettlementId { get; set; }
    public SettlementEngine? Settlement { get; set; }

    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }

    public decimal MeasuredKwh { get; set; }

    [MaxLength(4000)]
    public string? Notes { get; set; }
}
