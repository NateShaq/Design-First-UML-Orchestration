namespace HospitalApi.Domain;

public class InsurancePayer
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PlanType { get; set; } = string.Empty;
}

public class InsuranceClaim
{
    public Guid Id { get; set; }
    public Guid PatientRecordId { get; set; }
    public Guid PayerId { get; set; }
    public string ClaimNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class BillingInvoice
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateOnly InvoiceDate { get; set; }
    public bool Paid { get; set; }
}

public class Payment
{
    public Guid Id { get; set; }
    public Guid InvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidAt { get; set; }
    public string Method { get; set; } = string.Empty;
}
