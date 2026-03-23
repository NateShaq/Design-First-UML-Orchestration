using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml8.Api.Domain.Entities;

public class ComplianceCert
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public long Version { get; set; }

    [Required, MaxLength(100)]
    public string CertType { get; set; } = string.Empty;

    public DateTime Expiry { get; set; }

    [Timestamp]
    [Column(TypeName = "rowversion")]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    [Required]
    public string ProductId { get; set; } = string.Empty;
    public Product? Product { get; set; }
}
