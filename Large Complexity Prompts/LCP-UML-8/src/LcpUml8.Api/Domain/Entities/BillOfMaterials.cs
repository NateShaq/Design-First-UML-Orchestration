using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml8.Api.Domain.Entities;

public class BillOfMaterials
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    public long Version { get; set; }

    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;

    [Timestamp]
    [Column(TypeName = "rowversion")]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public ICollection<BillOfMaterialsItem> Items { get; set; } = new List<BillOfMaterialsItem>();
}

public class BillOfMaterialsItem
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string BillOfMaterialsId { get; set; } = string.Empty;
    public BillOfMaterials? BillOfMaterials { get; set; }

    [Required]
    public string ProductId { get; set; } = string.Empty;
    public Product? Product { get; set; }
}
