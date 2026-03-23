using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class LoadBalancer : ThreadSafeEntity
{
    [Key]
    public Guid BalancerId { get; set; }

    public Guid? GridConnectionId { get; set; }
    public GridConnection? GridConnection { get; set; }

    [MaxLength(200)]
    public string? DispatchPlan { get; set; }
}
