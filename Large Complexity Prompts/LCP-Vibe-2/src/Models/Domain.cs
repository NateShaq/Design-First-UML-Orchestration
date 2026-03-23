using System;
using System.Collections.Generic;
using System.Linq;

namespace LcpVibe2.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal UnitPrice { get; set; }
        public int QuantityOnHand { get; set; }
        public string Channel { get; set; } = "Omnichannel";
    }

    public class PerishableGoods : Product
    {
        public DateTime ExpirationDate { get; set; }
        public int ShelfLifeDays { get; set; }
        public string StorageRequirement { get; set; } = "Refrigerated";
    }

    public class LuxuryItems : Product
    {
        public decimal InsuranceValue { get; set; }
        public bool RequiresSignature { get; set; } = true;
        public string AuthenticationCertificate { get; set; } = string.Empty;
    }

    public class DigitalDownloads : Product
    {
        public string FileUrl { get; set; } = string.Empty;
        public int FileSizeMb { get; set; }
        public string LicenseKey { get; set; } = string.Empty;
    }

    public class Store
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public List<PointOfSaleTerminal> Terminals { get; set; } = new();
    }

    public class Warehouse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<StockMovement> Movements { get; set; } = new();
    }

    public class Supplier
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ContactEmail { get; set; } = string.Empty;
    }

    public class PurchaseOrder
    {
        public Guid Id { get; set; }
        public Guid SupplierId { get; set; }
        public DateTime OrderedAt { get; set; }
        public List<OrderLine> Lines { get; set; } = new();
    }

    public class SalesOrder
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public DateTime OrderedAt { get; set; } = DateTime.UtcNow;
        public string Channel { get; set; } = "ECommerce";
        public List<OrderLine> Lines { get; set; } = new();
        public decimal SubTotal => Lines.Sum(l => l.UnitPrice * l.Quantity);
        public decimal TaxTotal { get; set; }
        public decimal GrandTotal { get; set; }
    }

    public class OrderLine
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TaxAmount { get; set; }
    }

    public class Customer
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public Guid LoyaltyAccountId { get; set; }
    }

    public class LoyaltyProgram
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "Omnichannel Rewards";
        public decimal EarnRate { get; set; } = 0.01m; // points per dollar
        public decimal BurnRate { get; set; } = 0.01m; // dollar value per point
    }

    public class LoyaltyAccount
    {
        public Guid Id { get; set; }
        public Guid ProgramId { get; set; }
        public int PointsBalance { get; set; }
    }

    public class LoyaltyTransaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid AccountId { get; set; }
        public Guid SalesOrderId { get; set; }
        public int PointsEarned { get; set; }
        public int PointsRedeemed { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class Promotion
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public bool IsPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
    }

    public class ShoppingCart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid CustomerId { get; set; }
        public List<CartItem> Items { get; set; } = new();
    }

    public class CartItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class Payment
    {
        public Guid Id { get; set; }
        public Guid SalesOrderId { get; set; }
        public string Method { get; set; } = "Card";
        public decimal Amount { get; set; }
        public DateTime PaidAt { get; set; }
    }

    public class ReturnAuthorization
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid SalesOrderId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string ReasonCode { get; set; } = string.Empty;
        public List<ReturnLineItem> Items { get; set; } = new();
    }

    public class ReturnLineItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string Condition { get; set; } = "Resellable";
    }

    public class InventoryAudit
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public int Change { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class StockMovement
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid WarehouseId { get; set; }
        public int Quantity { get; set; }
        public string Direction { get; set; } = "Inbound"; // Inbound | Outbound | Transfer
        public DateTimeOffset OccurredAt { get; set; }
    }

    public class Shipment
    {
        public Guid Id { get; set; }
        public Guid SalesOrderId { get; set; }
        public string Carrier { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public DateTime ShippedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }

    public class DeliveryRoute
    {
        public Guid Id { get; set; }
        public string RouteCode { get; set; } = string.Empty;
        public List<Shipment> Shipments { get; set; } = new();
    }

    public class FulfillmentRequest
    {
        public Guid Id { get; set; }
        public Guid SalesOrderId { get; set; }
        public Guid WarehouseId { get; set; }
        public string Status { get; set; } = "Pending";
    }

    public class PricingRule
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string AppliesToChannel { get; set; } = "Omnichannel";
        public decimal Adjustment { get; set; }
        public bool IsPercentage { get; set; }
    }

    public class ChannelMapping
    {
        public Guid Id { get; set; }
        public string ExternalChannel { get; set; } = "Marketplace";
        public string InternalChannel { get; set; } = "ECommerce";
        public string SynchronizationMode { get; set; } = "Bidirectional";
    }

    public class PointOfSaleTerminal
    {
        public Guid Id { get; set; }
        public Guid StoreId { get; set; }
        public string TerminalCode { get; set; } = string.Empty;
        public string SoftwareVersion { get; set; } = "1.0.0";
    }

    public class ECommercePlatform
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "Web";
        public string Url { get; set; } = string.Empty;
        public bool SupportsDigitalGoods { get; set; }
    }

    public class AnalyticsReport
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }
        public Dictionary<string, decimal> Metrics { get; set; } = new();
    }
}
