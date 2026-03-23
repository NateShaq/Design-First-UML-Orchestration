namespace HospitalApi.Domain;

public class Patient
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
}

public class Inpatient : Patient
{
    public Guid AdmissionId { get; set; }
    public Guid RoomId { get; set; }
    public DateTime AdmittedOn { get; set; }
    public string AttendingPhysicianId { get; set; } = string.Empty;
}

public class Outpatient : Patient
{
    public string PrimaryClinic { get; set; } = string.Empty;
    public bool ActiveReferral { get; set; }
}

public class EmergencyCase : Patient
{
    public string TriageLevel { get; set; } = string.Empty;
    public DateTime ArrivalTime { get; set; }
    public string ChiefComplaint { get; set; } = string.Empty;
}

public class Appointment
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public Guid ProviderId { get; set; }
    public DateTime ScheduledFor { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class PatientRecord
{
    public Guid Id { get; set; }
    public Guid PatientId { get; set; }
    public string RecordNumber { get; set; } = string.Empty;
    public DateOnly EncounterDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}

public class VitalSign
{
    public Guid Id { get; set; }
    public Guid PatientRecordId { get; set; }
    public string Type { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public DateTime RecordedAt { get; set; }
}
