using LcpUml5.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LcpUml5.Api.Infrastructure;

public class BankingDbContext : DbContext
{
    public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options) { }

    public DbSet<CustomerProfile> CustomerProfiles => Set<CustomerProfile>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<SavingsAccount> SavingsAccounts => Set<SavingsAccount>();
    public DbSet<CryptoWallet> CryptoWallets => Set<CryptoWallet>();
    public DbSet<Portfolio> Portfolios => Set<Portfolio>();
    public DbSet<StockPortfolio> StockPortfolios => Set<StockPortfolio>();
    public DbSet<StockHolding> StockHoldings => Set<StockHolding>();
    public DbSet<TransactionLedger> TransactionLedgers => Set<TransactionLedger>();
    public DbSet<LedgerEntry> LedgerEntries => Set<LedgerEntry>();
    public DbSet<RiskAssessment> RiskAssessments => Set<RiskAssessment>();
    public DbSet<KycVerification> KycVerifications => Set<KycVerification>();
    public DbSet<TaxDocument> TaxDocuments => Set<TaxDocument>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().UseTptMappingStrategy();
        modelBuilder.Entity<Portfolio>().UseTptMappingStrategy();

        modelBuilder.Entity<CustomerProfile>()
            .HasIndex(x => x.CustomerId)
            .IsUnique();

        modelBuilder.Entity<Account>()
            .HasIndex(x => x.AccountNumber)
            .IsUnique();

        modelBuilder.Entity<Portfolio>()
            .HasIndex(x => x.PortfolioCode)
            .IsUnique();

        modelBuilder.Entity<TransactionLedger>()
            .HasIndex(x => x.LedgerId)
            .IsUnique();

        modelBuilder.Entity<CustomerProfile>()
            .HasMany(x => x.Accounts)
            .WithOne(x => x.CustomerProfile)
            .HasForeignKey(x => x.CustomerProfileId);

        modelBuilder.Entity<CustomerProfile>()
            .HasMany(x => x.Portfolios)
            .WithOne(x => x.CustomerProfile)
            .HasForeignKey(x => x.CustomerProfileId);

        modelBuilder.Entity<CustomerProfile>()
            .HasOne(x => x.KycVerification)
            .WithOne(x => x.CustomerProfile)
            .HasForeignKey<KycVerification>(x => x.CustomerProfileId);

        modelBuilder.Entity<StockHolding>()
            .HasOne(x => x.StockPortfolio)
            .WithMany(x => x.Holdings)
            .HasForeignKey(x => x.StockPortfolioId);

        modelBuilder.Entity<LedgerEntry>()
            .HasOne(x => x.TransactionLedger)
            .WithMany(x => x.Entries)
            .HasForeignKey(x => x.TransactionLedgerId);
    }
}
