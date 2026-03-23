using System.ComponentModel.DataAnnotations;

namespace LcpUml6.Api.Domain.Entities;

/// <summary>
/// Base for optimistic concurrency protected aggregates that must block ghost writes.
/// </summary>
public abstract class ThreadSafeEntity
{
    public int Revision { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
