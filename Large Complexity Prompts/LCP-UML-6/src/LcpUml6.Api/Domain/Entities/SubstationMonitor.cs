using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class SubstationMonitor : ThreadSafeEntity
{
    [Key]
    public Guid MonitorId { get; set; }

    public Guid? GridConnectionId { get; set; }
    public GridConnection? GridConnection { get; set; }

    [MaxLength(200)]
    public string? BreakerState { get; set; }
}
