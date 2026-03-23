namespace LcpUml5.Api.Contracts.Requests;

public record CreateCustomerProfileRequest(string CustomerId, string Segment, string RiskLevel);

public record CreateSavingsAccountRequest(int CustomerProfileId, string AccountNumber, decimal InitialDeposit, decimal InterestRate, string Currency = "USD", string? IdempotencyKey = null);

public record CreateCryptoWalletRequest(int CustomerProfileId, string AccountNumber, string Chain, string PublicAddress, string Currency = "USD", string? IdempotencyKey = null);

public record MoneyMovementRequest(decimal Amount, string? IdempotencyKey = null);

public record PostLedgerEntryRequest(decimal Amount, string EntryType, string Reference, string? IdempotencyKey = null);

public record CreateRiskAssessmentRequest(int CustomerProfileId, decimal Score, string? IdempotencyKey = null);

public record CreateTaxDocumentRequest(int CustomerProfileId, int TaxYear, string? IdempotencyKey = null);
