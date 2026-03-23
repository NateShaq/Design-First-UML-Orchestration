using System;
using System.ComponentModel.DataAnnotations;

namespace FleetNetworkApi.Models;

public abstract class BaseEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Optimistic concurrency token to prevent ghost writes.
    /// </summary>
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    /// <summary>
    /// Optional logical lock token mirroring UML lockToken notes.
    /// </summary>
    [MaxLength(64)]
    public string? LockToken { get; set; }
}
