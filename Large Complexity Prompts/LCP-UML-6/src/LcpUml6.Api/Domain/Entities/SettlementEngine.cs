using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class SettlementEngine
{
    [Key]
    public Guid SettlementId { get; set; }

    [MaxLength(80)]
    public string Status { get; set; } = "Pending";
}
