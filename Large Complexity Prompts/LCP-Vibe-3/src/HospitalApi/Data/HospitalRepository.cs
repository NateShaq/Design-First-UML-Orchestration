using HospitalApi.Domain;

namespace HospitalApi.Data;

public class HospitalRepository
{
    public List<PatientRecord> PatientRecords { get; } = new();
    public List<PharmacyItem> PharmacyInventory { get; } = new();
    public List<SurgicalCase> SurgicalCases { get; } = new();

    public HospitalRepository()
    {
        // Seed a few entities for demo purposes
        var patient = new Patient { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Doe", DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-34)) };
        PatientRecords.Add(new PatientRecord
        {
            Id = Guid.NewGuid(),
            PatientId = patient.Id,
            RecordNumber = "PR-1001",
            EncounterDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Status = "Open",
            Notes = "Initial intake for chest pain"
        });

        PharmacyInventory.Add(new PharmacyItem
        {
            Id = Guid.NewGuid(),
            MedicationCode = "AMOX500",
            Name = "Amoxicillin 500mg",
            QuantityOnHand = 120,
            ExpirationDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(6))
        });

        SurgicalCases.Add(new SurgicalCase
        {
            Id = Guid.NewGuid(),
            PatientId = patient.Id,
            ProcedureCode = "KNEE-ARTH",
            ScheduledStart = DateTime.UtcNow.AddDays(3),
            OperatingRoomId = Guid.NewGuid(),
            Status = "Scheduled"
        });
    }
}
