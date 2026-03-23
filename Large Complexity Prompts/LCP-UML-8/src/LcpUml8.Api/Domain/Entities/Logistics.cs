using System.ComponentModel.DataAnnotations;

namespace LcpUml8.Api.Domain.Entities;

public class InventoryLot
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(100)]
    public string LotNo { get; set; } = string.Empty;

    public DateTime Expiry { get; set; }

    public string? RawMaterialId { get; set; }
    public RawMaterial? RawMaterial { get; set; }

    public string? SubAssemblyId { get; set; }
    public SubAssembly? SubAssembly { get; set; }

    [Required]
    public string WarehouseId { get; set; } = string.Empty;
    public Warehouse? Warehouse { get; set; }

    public ICollection<TraceabilityRecord> TraceabilityRecords { get; set; } = new List<TraceabilityRecord>();
}

public class Warehouse
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    public ICollection<InventoryLot> InventoryLots { get; set; } = new List<InventoryLot>();
}

public class CustomerOrder
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(100)]
    public string OrderNo { get; set; } = string.Empty;

    [Required]
    public string FinishedProductId { get; set; } = string.Empty;
    public FinishedProduct? FinishedProduct { get; set; }

    public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
}

public class Shipment
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(100)]
    public string Tracking { get; set; } = string.Empty;

    [Required]
    public string CustomerOrderId { get; set; } = string.Empty;
    public CustomerOrder? CustomerOrder { get; set; }

    public ICollection<TraceabilityRecord> TraceabilityRecords { get; set; } = new List<TraceabilityRecord>();
}

public class TraceabilityRecord
{
    [Key]
    [MaxLength(64)]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    [Required, MaxLength(200)]
    public string Event { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    public string? InventoryLotId { get; set; }
    public InventoryLot? InventoryLot { get; set; }

    public string? WorkOrderId { get; set; }
    public WorkOrder? WorkOrder { get; set; }

    public string? InspectionResultId { get; set; }
    public InspectionResult? InspectionResult { get; set; }

    public string? ShipmentId { get; set; }
    public Shipment? Shipment { get; set; }
}
