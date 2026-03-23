namespace LcpUml6.Api.Contracts.Requests;

public class CreateSettlementRequest
{
    public Guid? SettlementId { get; set; }
    public string Status { get; set; } = "Pending";
}
