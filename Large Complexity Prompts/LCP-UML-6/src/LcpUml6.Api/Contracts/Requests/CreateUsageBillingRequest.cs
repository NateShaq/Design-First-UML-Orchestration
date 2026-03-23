namespace LcpUml6.Api.Contracts.Requests;

public class CreateUsageBillingRequest
{
    public Guid? BillingId { get; set; }
    public Guid MeterId { get; set; }
    public Guid TariffPlanId { get; set; }
    public Guid SettlementId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public decimal MeasuredKwh { get; set; }
    public string? Notes { get; set; }
}
