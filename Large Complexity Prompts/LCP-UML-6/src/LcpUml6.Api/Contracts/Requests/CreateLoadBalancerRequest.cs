namespace LcpUml6.Api.Contracts.Requests;

public class CreateLoadBalancerRequest
{
    public Guid? BalancerId { get; set; }
    public Guid? GridConnectionId { get; set; }
    public string? DispatchPlan { get; set; }
}
