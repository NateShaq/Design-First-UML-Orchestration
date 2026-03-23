using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

public class TariffPlan
{
    [Key]
    public Guid TariffPlanId { get; set; }

    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(4000)]
    public string RateStructure { get; set; } = string.Empty;
}
