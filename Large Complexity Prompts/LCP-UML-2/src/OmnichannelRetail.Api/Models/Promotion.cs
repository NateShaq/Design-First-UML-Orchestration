using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmnichannelRetail.Api.Models;

public class PromotionEngine
{
    [Key]
    public Guid PromotionEngineId { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<DiscountRule> DiscountRules { get; set; } = new List<DiscountRule>();

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class DiscountRule
{
    [Key]
    public Guid RuleId { get; set; }

    public Guid PromotionEngineId { get; set; }
    public PromotionEngine? PromotionEngine { get; set; }

    [MaxLength(300)]
    public string? Description { get; set; }

    public double Percentage { get; set; }
}
