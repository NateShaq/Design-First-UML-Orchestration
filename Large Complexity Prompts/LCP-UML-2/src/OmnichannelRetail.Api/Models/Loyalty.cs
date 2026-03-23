using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmnichannelRetail.Api.Models;

public class LoyaltyProgram
{
    [Key]
    public Guid LoyaltyProgramId { get; set; }

    [Required, MaxLength(100)]
    public string ProgramName { get; set; } = string.Empty;

    public long Version { get; set; }

    public Guid? LockToken { get; set; }

    public ICollection<MembershipTier> Tiers { get; set; } = new List<MembershipTier>();
    public ICollection<LoyaltyAccount> Accounts { get; set; } = new List<LoyaltyAccount>();

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class LoyaltyAccount
{
    [Key]
    public Guid AccountId { get; set; }

    public Guid LoyaltyProgramId { get; set; }
    public LoyaltyProgram? LoyaltyProgram { get; set; }

    public Guid CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public int PointsBalance { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class MembershipTier
{
    [Key]
    public Guid MembershipTierId { get; set; }

    public Guid LoyaltyProgramId { get; set; }
    public LoyaltyProgram? LoyaltyProgram { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public int Threshold { get; set; }

    [MaxLength(500)]
    public string? Benefits { get; set; }
}
