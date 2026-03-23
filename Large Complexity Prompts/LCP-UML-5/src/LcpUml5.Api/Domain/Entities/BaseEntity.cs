using System.ComponentModel.DataAnnotations;

namespace LcpUml5.Api.Domain.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }

    // SQL Server rowversion column for ghost-write protection/concurrency.
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
