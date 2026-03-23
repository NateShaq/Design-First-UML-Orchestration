using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OmnichannelRetail.Api.Models;

public class StockLocation
{
    [Key]
    public Guid LocationId { get; set; }

    [Required, MaxLength(300)]
    public string Address { get; set; } = string.Empty;

    public string Discriminator { get; set; } = "Generic";

    public Inventory? Inventory { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class Warehouse : StockLocation
{
    public int Capacity { get; set; }
    [MaxLength(100)]
    public string? AutomationLevel { get; set; }
}

public class Store : StockLocation
{
    [MaxLength(50)]
    public string? StoreNumber { get; set; }
    public int PosCount { get; set; }
}

public class Inventory
{
    [Key]
    public Guid InventoryId { get; set; }

    public Guid LocationId { get; set; }
    public StockLocation? Location { get; set; }

    public int ReorderPoint { get; set; }

    public ICollection<InventoryItem> Items { get; set; } = new List<InventoryItem>();

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class InventoryItem
{
    [Key]
    public Guid InventoryItemId { get; set; }

    public Guid InventoryId { get; set; }
    public Inventory? Inventory { get; set; }

    [Required, MaxLength(50)]
    public string ProductSku { get; set; } = string.Empty;
    public Product? Product { get; set; }

    public int QuantityOnHand { get; set; }
    public int Reserved { get; set; }
}

public class Supplier
{
    [Key]
    public Guid SupplierId { get; set; }

    public int LeadTimeDays { get; set; }

    [MaxLength(50)]
    public string? Rating { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class Shipment
{
    [Key]
    public Guid ShipmentId { get; set; }

    public Guid? WarehouseId { get; set; }
    public Warehouse? Warehouse { get; set; }

    [MaxLength(100)]
    public string? Carrier { get; set; }

    [MaxLength(100)]
    public string? TrackingNumber { get; set; }

    public Guid? DeliveryScheduleId { get; set; }
    public DeliverySchedule? DeliverySchedule { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}

public class DeliverySchedule
{
    [Key]
    public Guid DeliveryScheduleId { get; set; }

    public DateTime WindowStart { get; set; }
    public DateTime WindowEnd { get; set; }

    [MaxLength(50)]
    public string? Priority { get; set; }
}

public class InventoryAudit
{
    [Key]
    public Guid AuditId { get; set; }

    public Guid InventoryId { get; set; }
    public Inventory? Inventory { get; set; }

    public DateOnly ScheduledDate { get; set; }
    public bool VarianceDetected { get; set; }

    public long Version { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
