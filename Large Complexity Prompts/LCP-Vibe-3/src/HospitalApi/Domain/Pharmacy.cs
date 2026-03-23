namespace HospitalApi.Domain;

public class Medication
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Strength { get; set; } = string.Empty;
}

public class PharmacyItem
{
    public Guid Id { get; set; }
    public string MedicationCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int QuantityOnHand { get; set; }
    public DateOnly ExpirationDate { get; set; }
}

public class Prescription
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string MedicationCode { get; set; } = string.Empty;
    public string Sig { get; set; } = string.Empty;
    public int DispenseQuantity { get; set; }
}

public class DispenseEvent
{
    public Guid Id { get; set; }
    public Guid PharmacyItemId { get; set; }
    public Guid PatientId { get; set; }
    public int Quantity { get; set; }
    public DateTime DispensedAt { get; set; }
}

public class Supplier
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
}
