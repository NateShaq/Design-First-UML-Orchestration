namespace LcpUml6.Api.Contracts.Requests;

public class CreateSmartMeterRequest
{
    public Guid? MeterId { get; set; }
    public Guid CustomerId { get; set; }
    public Guid? GridConnectionId { get; set; }
}
