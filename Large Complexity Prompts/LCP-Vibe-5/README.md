# LCP-Vibe-5 – Digital Banking & Wealth Management Engine

Minimal .NET 8 Web API plus domain model (30 classes) and SQL schema covering retail banking, investment portfolios, and compliance monitoring.

## Run the API
```bash
cd LCP-Vibe-5/Api
DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1 dotnet restore
dotnet run
```
Browse Swagger at http://localhost:5000/swagger.

## Endpoints (seeded in-memory)
- `GET /api/accounts` – savings accounts
- `GET /api/portfolios` – stock portfolios
- `GET /api/compliance/risk` – risk assessments
- `GET /api/compliance/kyc` – KYC status
- `GET /api/transactions` – transaction ledger
- `POST /api/transactions` – add a transaction

## Domain coverage (30 classes)
Retail Banking: SavingsAccount, CurrentAccount, LoanAccount, TransactionLedger, PaymentInstruction, Card, Branch, CustomerProfile, ContactInfo, Beneficiary.
Investment: StockPortfolio, BondPortfolio, MutualFund, CryptoWallet, InvestmentAllocation, TradeOrder, CorporateAction, PerformanceMetric, FeeSchedule, RebalancingPolicy.
Compliance: RiskAssessment, KYCVerification, AMLAlert, TransactionMonitoringRule, AuditTrail, TaxDocument, RegulatoryReport, SanctionListEntry, ConsentRecord, DataRetentionPolicy.

## Database schema
See `sql/schema.sql` for table definitions aligned to the domain objects.
