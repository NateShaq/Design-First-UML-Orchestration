using Microsoft.EntityFrameworkCore;
using OmnichannelRetail.Api.Models;

namespace OmnichannelRetail.Api.Data;

public class RetailDbContext : DbContext
{
    public RetailDbContext(DbContextOptions<RetailDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<StockLocation> StockLocations => Set<StockLocation>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Store> Stores => Set<Store>();
    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<DeliverySchedule> DeliverySchedules => Set<DeliverySchedule>();
    public DbSet<InventoryAudit> InventoryAudits => Set<InventoryAudit>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<PaymentGateway> PaymentGateways => Set<PaymentGateway>();
    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    public DbSet<RefundProcessor> RefundProcessors => Set<RefundProcessor>();
    public DbSet<ReturnAuthorization> ReturnAuthorizations => Set<ReturnAuthorization>();
    public DbSet<LoyaltyProgram> LoyaltyPrograms => Set<LoyaltyProgram>();
    public DbSet<LoyaltyAccount> LoyaltyAccounts => Set<LoyaltyAccount>();
    public DbSet<MembershipTier> MembershipTiers => Set<MembershipTier>();
    public DbSet<PromotionEngine> PromotionEngines => Set<PromotionEngine>();
    public DbSet<DiscountRule> DiscountRules => Set<DiscountRule>();
    public DbSet<EcommercePlatform> EcommercePlatforms => Set<EcommercePlatform>();
    public DbSet<POSSystem> PosSystems => Set<POSSystem>();
    public DbSet<SupplyChainLogistics> SupplyChainLogistics => Set<SupplyChainLogistics>();
    public DbSet<TaxCalculator> TaxCalculators => Set<TaxCalculator>();
    public DbSet<AnalyticsService> AnalyticsServices => Set<AnalyticsService>();
    public DbSet<NotificationService> NotificationServices => Set<NotificationService>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasDiscriminator<string>(p => p.Discriminator)
            .HasValue<Product>("Standard")
            .HasValue<PerishableGoods>("Perishable")
            .HasValue<LuxuryItems>("Luxury")
            .HasValue<DigitalDownloads>("Digital");

        modelBuilder.Entity<StockLocation>().HasDiscriminator<string>(s => s.Discriminator)
            .HasValue<StockLocation>("Generic")
            .HasValue<Warehouse>("Warehouse")
            .HasValue<Store>("Store");

        modelBuilder.Entity<OrderLine>()
            .HasOne(l => l.Order)
            .WithMany(o => o.Lines)
            .HasForeignKey(l => l.OrderNumber);

        modelBuilder.Entity<OrderLine>()
            .HasOne(l => l.Product)
            .WithMany()
            .HasForeignKey(l => l.ProductSku);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Cart)
            .WithMany(c => c.Items)
            .HasForeignKey(ci => ci.CartId);

        modelBuilder.Entity<CartItem>()
            .HasOne(ci => ci.Product)
            .WithMany()
            .HasForeignKey(ci => ci.ProductSku);

        modelBuilder.Entity<InventoryItem>()
            .HasOne(ii => ii.Inventory)
            .WithMany(i => i.Items)
            .HasForeignKey(ii => ii.InventoryId);

        modelBuilder.Entity<InventoryItem>()
            .HasOne(ii => ii.Product)
            .WithMany()
            .HasForeignKey(ii => ii.ProductSku);

        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Location)
            .WithOne(l => l.Inventory)
            .HasForeignKey<Inventory>(i => i.LocationId);

        modelBuilder.Entity<LoyaltyAccount>()
            .HasOne(a => a.Customer)
            .WithOne(c => c.LoyaltyAccount)
            .HasForeignKey<LoyaltyAccount>(a => a.CustomerId);

        modelBuilder.Entity<LoyaltyAccount>()
            .HasOne(a => a.LoyaltyProgram)
            .WithMany(p => p.Accounts)
            .HasForeignKey(a => a.LoyaltyProgramId);

        modelBuilder.Entity<MembershipTier>()
            .HasOne(t => t.LoyaltyProgram)
            .WithMany(p => p.Tiers)
            .HasForeignKey(t => t.LoyaltyProgramId);

        modelBuilder.Entity<PromotionEngine>()
            .HasMany(p => p.DiscountRules)
            .WithOne(r => r.PromotionEngine)
            .HasForeignKey(r => r.PromotionEngineId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.PaymentTransaction)
            .WithOne(t => t.Order)
            .HasForeignKey<Order>(o => o.PaymentTransactionId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Shipment)
            .WithMany();

        modelBuilder.Entity<Order>()
            .HasOne(o => o.ReturnAuthorization)
            .WithMany();

        modelBuilder.Entity<PaymentTransaction>()
            .HasOne(t => t.PaymentGateway)
            .WithMany();
    }
}
