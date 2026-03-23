using System;
using System.Collections.Generic;
using System.Linq;
using LcpVibe5.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/accounts", () => SampleData.Accounts);

app.MapGet("/api/portfolios", () => SampleData.Portfolios);

app.MapGet("/api/compliance/risk", () => SampleData.RiskAssessments);

app.MapGet("/api/compliance/kyc", () => SampleData.KycItems);

app.MapPost("/api/transactions", (TransactionLedger ledger) =>
{
    ledger.Id = Guid.NewGuid();
    ledger.Timestamp = DateTime.UtcNow;
    SampleData.Transactions.Add(ledger);
    return Results.Created($"/api/transactions/{ledger.Id}", ledger);
});

app.MapGet("/api/transactions", () => SampleData.Transactions);

app.Run();

namespace LcpVibe5.Domain
{
    // In-memory seed data; replace with repository/DB in production
    public static class SampleData
    {
        public static List<SavingsAccount> Accounts { get; } = new()
        {
            new SavingsAccount { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Balance = 12500m, InterestRate = 0.02m, Currency = "USD" },
            new SavingsAccount { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Balance = 8300m, InterestRate = 0.018m, Currency = "USD" }
        };

        public static List<StockPortfolio> Portfolios { get; } = new()
        {
            new StockPortfolio { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Name = "Core Equity", Holdings = new Dictionary<string, decimal>{{"AAPL", 120}, {"MSFT", 80}}, BaseCurrency = "USD" },
            new StockPortfolio { Id = Guid.NewGuid(), CustomerId = Guid.NewGuid(), Name = "Growth Tech", Holdings = new Dictionary<string, decimal>{{"NVDA", 50}, {"AMZN", 30}}, BaseCurrency = "USD" }
        };

        public static List<RiskAssessment> RiskAssessments { get; } = new()
        {
            new RiskAssessment { Id = Guid.NewGuid(), CustomerId = Accounts.First().CustomerId, Score = 62, Category = "Moderate", AssessedOn = DateTime.UtcNow.AddDays(-14) },
            new RiskAssessment { Id = Guid.NewGuid(), CustomerId = Accounts.Last().CustomerId, Score = 78, Category = "Growth", AssessedOn = DateTime.UtcNow.AddDays(-7) }
        };

        public static List<KYCVerification> KycItems { get; } = new()
        {
            new KYCVerification { Id = Guid.NewGuid(), CustomerId = Accounts.First().CustomerId, Status = "Completed", VerifiedOn = DateTime.UtcNow.AddDays(-30) },
            new KYCVerification { Id = Guid.NewGuid(), CustomerId = Accounts.Last().CustomerId, Status = "Pending", VerifiedOn = null }
        };

        public static List<TransactionLedger> Transactions { get; } = new();
    }
}
