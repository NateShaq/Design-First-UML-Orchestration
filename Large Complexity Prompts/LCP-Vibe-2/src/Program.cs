using LcpVibe2.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IInventoryService, InMemoryInventoryService>();
builder.Services.AddSingleton<ITaxCalculator, TaxCalculator>();
builder.Services.AddSingleton<ILoyaltyEngine, LoyaltyEngine>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.MapGet("/products", (IInventoryService inventory) => inventory.GetAll());
app.MapPost("/orders", (SalesOrder order, IInventoryService inventory, ITaxCalculator taxCalculator) =>
{
    var taxed = taxCalculator.Calculate(order);
    inventory.CommitSalesOrder(taxed);
    return Results.Created($"/orders/{order.Id}", taxed);
});

app.MapPost("/returns", (ReturnAuthorization ra, IInventoryService inventory) =>
{
    inventory.ProcessReturn(ra);
    return Results.Ok(ra);
});

app.MapPost("/loyalty/accrue", (LoyaltyTransaction tx, ILoyaltyEngine loyalty) =>
{
    loyalty.Apply(tx);
    return Results.Ok(tx);
});

app.Run();

public interface IInventoryService
{
    IReadOnlyCollection<Product> GetAll();
    void CommitSalesOrder(SalesOrder order);
    void ProcessReturn(ReturnAuthorization authorization);
}

public class InMemoryInventoryService : IInventoryService
{
    private readonly Dictionary<Guid, Product> _catalog = new();
    private readonly List<InventoryAudit> _audits = new();

    public InMemoryInventoryService()
    {
        Seed();
    }

    public IReadOnlyCollection<Product> GetAll() => _catalog.Values.ToList();

    public void CommitSalesOrder(SalesOrder order)
    {
        foreach (var line in order.Lines)
        {
            if (_catalog.TryGetValue(line.ProductId, out var product))
            {
                product.QuantityOnHand = Math.Max(0, product.QuantityOnHand - line.Quantity);
                _audits.Add(new InventoryAudit
                {
                    Id = Guid.NewGuid(),
                    ProductId = line.ProductId,
                    Timestamp = DateTimeOffset.UtcNow,
                    Change = -line.Quantity,
                    Reason = "Sale"
                });
            }
        }
    }

    public void ProcessReturn(ReturnAuthorization authorization)
    {
        foreach (var line in authorization.Items)
        {
            if (_catalog.TryGetValue(line.ProductId, out var product))
            {
                product.QuantityOnHand += line.Quantity;
                _audits.Add(new InventoryAudit
                {
                    Id = Guid.NewGuid(),
                    ProductId = line.ProductId,
                    Timestamp = DateTimeOffset.UtcNow,
                    Change = line.Quantity,
                    Reason = "Return"
                });
            }
        }
    }

    private void Seed()
    {
        var milk = new PerishableGoods
        {
            Id = Guid.NewGuid(),
            Sku = "MILK-1L",
            Name = "1L Whole Milk",
            ExpirationDate = DateTime.UtcNow.AddDays(7),
            QuantityOnHand = 120,
            ShelfLifeDays = 10
        };
        _catalog[milk.Id] = milk;

        var bag = new LuxuryItems
        {
            Id = Guid.NewGuid(),
            Sku = "LUX-BAG-01",
            Name = "Heritage Leather Bag",
            QuantityOnHand = 5,
            InsuranceValue = 2500m
        };
        _catalog[bag.Id] = bag;

        var album = new DigitalDownloads
        {
            Id = Guid.NewGuid(),
            Sku = "DIG-ALB-99",
            Name = "Live Concert Album",
            FileSizeMb = 512,
            LicenseKey = Guid.NewGuid().ToString()
        };
        _catalog[album.Id] = album;
    }
}

public interface ITaxCalculator
{
    SalesOrder Calculate(SalesOrder order);
}

public class TaxCalculator : ITaxCalculator
{
    public SalesOrder Calculate(SalesOrder order)
    {
        const decimal rate = 0.0825m;
        foreach (var line in order.Lines)
        {
            line.TaxAmount = Math.Round(line.UnitPrice * line.Quantity * rate, 2);
        }
        order.TaxTotal = order.Lines.Sum(l => l.TaxAmount);
        order.GrandTotal = order.SubTotal + order.TaxTotal;
        return order;
    }
}

public interface ILoyaltyEngine
{
    void Apply(LoyaltyTransaction transaction);
}

public class LoyaltyEngine : ILoyaltyEngine
{
    private readonly Dictionary<Guid, LoyaltyAccount> _accounts = new();

    public void Apply(LoyaltyTransaction transaction)
    {
        if (!_accounts.TryGetValue(transaction.AccountId, out var account))
        {
            account = new LoyaltyAccount { Id = transaction.AccountId, PointsBalance = 0 };
            _accounts[transaction.AccountId] = account;
        }
        account.PointsBalance += transaction.PointsEarned - transaction.PointsRedeemed;
    }
}
