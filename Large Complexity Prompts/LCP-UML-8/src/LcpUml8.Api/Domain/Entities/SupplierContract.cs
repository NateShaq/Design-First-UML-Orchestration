using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml8.Api.Domain.Entities;

public class SupplierContract
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public long Version { get; set; }

    [Required, MaxLength(500)]
    public string Terms { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Incoterms { get; set; } = string.Empty;

    [Timestamp]
    [Column(TypeName = "rowversion")]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    [Required]
    public string RawMaterialId { get; set; } = string.Empty;
    public RawMaterial? RawMaterial { get; set; }
}
