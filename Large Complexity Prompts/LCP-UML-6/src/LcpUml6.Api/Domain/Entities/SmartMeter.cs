using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class SmartMeter
{
    [Key]
    public Guid MeterId { get; set; }

    public Guid CustomerId { get; set; }

    public Guid? GridConnectionId { get; set; }
    public GridConnection? GridConnection { get; set; }
}
