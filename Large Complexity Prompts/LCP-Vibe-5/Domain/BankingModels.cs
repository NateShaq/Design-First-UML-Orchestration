using System;

namespace LcpVibe5.Domain;

public class SavingsAccount
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Balance { get; set; }
    public decimal InterestRate { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime OpenedOn { get; set; } = DateTime.UtcNow;
}

public class CurrentAccount
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal AvailableBalance { get; set; }
    public decimal OverdraftLimit { get; set; }
    public string Currency { get; set; } = "USD";
}

public class LoanAccount
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal Principal { get; set; }
    public decimal InterestRate { get; set; }
    public int TermMonths { get; set; }
    public DateTime DisbursedOn { get; set; }
}

public class TransactionLedger
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public DateTime Timestamp { get; set; }
    public string Type { get; set; } = "debit"; // debit or credit
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string Description { get; set; } = string.Empty;
}

public class PaymentInstruction
{
    public Guid Id { get; set; }
    public Guid FromAccountId { get; set; }
    public Guid ToAccountId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime ScheduledFor { get; set; }
    public string Status { get; set; } = "pending";
}

public class Card
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public string MaskedPan { get; set; } = string.Empty;
    public DateTime Expiry { get; set; }
    public bool ContactlessEnabled { get; set; }
}

public class Branch
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string RoutingNumber { get; set; } = string.Empty;
}

public class CustomerProfile
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public ContactInfo Contact { get; set; } = new();
    public string Segment { get; set; } = "Retail";
}

public class ContactInfo
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
}

public class Beneficiary
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public decimal AllocationPercentage { get; set; }
}
