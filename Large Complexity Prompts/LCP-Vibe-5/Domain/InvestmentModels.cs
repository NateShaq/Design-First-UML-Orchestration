using System;
using System.Collections.Generic;

namespace LcpVibe5.Domain;

public class StockPortfolio
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, decimal> Holdings { get; set; } = new();
    public string BaseCurrency { get; set; } = "USD";
}

public class BondPortfolio
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Dictionary<string, decimal> Holdings { get; set; } = new();
    public string BaseCurrency { get; set; } = "USD";
}

public class MutualFund
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ISIN { get; set; } = string.Empty;
    public decimal Nav { get; set; }
    public string Currency { get; set; } = "USD";
}

public class CryptoWallet
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Address { get; set; } = string.Empty;
    public Dictionary<string, decimal> Balances { get; set; } = new();
    public string Network { get; set; } = "Ethereum";
}

public class InvestmentAllocation
{
    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public string AssetClass { get; set; } = string.Empty;
    public decimal TargetPercentage { get; set; }
}

public class TradeOrder
{
    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public string Ticker { get; set; } = string.Empty;
    public string Side { get; set; } = "buy"; // buy or sell
    public int Quantity { get; set; }
    public decimal LimitPrice { get; set; }
    public string Status { get; set; } = "new";
}

public class CorporateAction
{
    public Guid Id { get; set; }
    public string Ticker { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // split, dividend, etc
    public DateTime EffectiveDate { get; set; }
    public decimal Ratio { get; set; }
}

public class PerformanceMetric
{
    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public decimal ReturnYtd { get; set; }
    public decimal Volatility { get; set; }
    public decimal SharpeRatio { get; set; }
    public DateTime CalculatedOn { get; set; }
}

public class FeeSchedule
{
    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public decimal ManagementFee { get; set; }
    public decimal PerformanceFee { get; set; }
    public decimal EntryLoad { get; set; }
}

public class RebalancingPolicy
{
    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public string Frequency { get; set; } = "Quarterly";
    public decimal DriftThreshold { get; set; }
    public DateTime LastRebalancedOn { get; set; }
}
