namespace HospitalApi.Domain;

public class LabResult
{
    public Guid Id { get; set; }
    public Guid PatientRecordId { get; set; }
    public string TestCode { get; set; } = string.Empty;
    public string ResultValue { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
    public DateTime ResultedAt { get; set; }
}

public class RadiologyImage
{
    public Guid Id { get; set; }
    public Guid PatientRecordId { get; set; }
    public string Modality { get; set; } = string.Empty;
    public string ImageUri { get; set; } = string.Empty;
    public DateTime CapturedAt { get; set; }
}
