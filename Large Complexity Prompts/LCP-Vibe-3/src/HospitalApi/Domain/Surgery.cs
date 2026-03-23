namespace HospitalApi.Domain;

public class Surgery
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class OperatingRoom
{
    public Guid Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public string Wing { get; set; } = string.Empty;
}

public class SurgicalCase
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string ProcedureCode { get; set; } = string.Empty;
    public Guid OperatingRoomId { get; set; }
    public DateTime ScheduledStart { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class AnesthesiaRecord
{
    public Guid Id { get; set; }
    public Guid SurgicalCaseId { get; set; }
    public string Anesthesiologist { get; set; } = string.Empty;
    public string Technique { get; set; } = string.Empty;
    public string Airway { get; set; } = string.Empty;
}

public class ProcedureCode
{
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ServiceLine { get; set; } = string.Empty;
}

public class PreOpAssessment
{
    public Guid Id { get; set; }
    public Guid SurgicalCaseId { get; set; }
    public string AsaClass { get; set; } = string.Empty;
    public string Findings { get; set; } = string.Empty;
}

public class PostOpNote
{
    public Guid Id { get; set; }
    public Guid SurgicalCaseId { get; set; }
    public string Outcome { get; set; } = string.Empty;
    public string Disposition { get; set; } = string.Empty;
}
