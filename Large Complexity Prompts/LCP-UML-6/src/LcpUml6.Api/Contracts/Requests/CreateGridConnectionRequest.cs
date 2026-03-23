namespace LcpUml6.Api.Contracts.Requests;

public class CreateGridConnectionRequest
{
    public Guid? GridConnectionId { get; set; }
    public string Node { get; set; } = string.Empty;
}
