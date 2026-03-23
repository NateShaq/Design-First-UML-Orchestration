using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml8.Api.Domain.Entities;

public abstract class Product
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Sku { get; set; } = string.Empty;

    public long Version { get; set; }

    [Timestamp]
    [Column(TypeName = "rowversion")]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public ICollection<ComplianceCert> ComplianceCerts { get; set; } = new List<ComplianceCert>();
}

public class RawMaterial : Product
{
    public ICollection<SupplierContract> SupplierContracts { get; set; } = new List<SupplierContract>();
    public ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();
}

public class SubAssembly : Product
{
    public ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();
}

public class FinishedProduct : Product
{
    public ICollection<CustomerOrder> CustomerOrders { get; set; } = new List<CustomerOrder>();
}
