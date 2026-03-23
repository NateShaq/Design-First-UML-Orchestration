namespace LcpUml6.Api.Contracts.Requests;

public class CreateTariffPlanRequest
{
    public Guid? TariffPlanId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RateStructure { get; set; } = string.Empty;
}
