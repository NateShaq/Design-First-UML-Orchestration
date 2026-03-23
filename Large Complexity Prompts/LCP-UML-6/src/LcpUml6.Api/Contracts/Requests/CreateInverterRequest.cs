namespace LcpUml6.Api.Contracts.Requests;

public class CreateInverterRequest
{
    public Guid? InverterId { get; set; }
    public string Model { get; set; } = string.Empty;
    public string FirmwareVersion { get; set; } = "1.0.0";
}
