namespace LcpUml6.Api.Contracts.Requests;

public class CreateSubstationMonitorRequest
{
    public Guid? MonitorId { get; set; }
    public Guid? GridConnectionId { get; set; }
    public string? BreakerState { get; set; }
}
