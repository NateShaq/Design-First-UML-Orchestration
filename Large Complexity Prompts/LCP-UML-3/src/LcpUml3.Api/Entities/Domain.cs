using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LcpUml3.Api.Entities;

public class Hospital
{
    public Guid HospitalId { get; set; } = Guid.NewGuid();
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public ICollection<Department> Departments { get; set; } = new List<Department>();
    public ICollection<EmrSystem> EmrSystems { get; set; } = new List<EmrSystem>();
}

public class Department
{
    public Guid DepartmentId { get; set; } = Guid.NewGuid();
    [Required]
    public string Code { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public Guid HospitalId { get; set; }
    public Hospital? Hospital { get; set; }
    public ICollection<Ward> Wards { get; set; } = new List<Ward>();
}

public class EmrSystem
{
    public Guid SystemId { get; set; } = Guid.NewGuid();
    public string Version { get; set; } = string.Empty;
    public string TxIsolation { get; set; } = string.Empty;
    public bool AuditTrailEnabled { get; set; }
    public Guid HospitalId { get; set; }
    public Hospital? Hospital { get; set; }
    public ICollection<PatientRecord> PatientRecords { get; set; } = new List<PatientRecord>();
    public ICollection<PharmacyInventory> PharmacyInventories { get; set; } = new List<PharmacyInventory>();
}

public class Patient
{
    [Key]
    public string PatientId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateOnly Dob { get; set; }
    public ICollection<PatientRecord> PatientRecords { get; set; } = new List<PatientRecord>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<BillingInvoice> BillingInvoices { get; set; } = new List<BillingInvoice>();
}

public class PatientRecord
{
    [Key]
    public string RecordId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PatientId { get; set; } = string.Empty;
    public Patient? Patient { get; set; }
    public Guid EmrSystemId { get; set; }
    public EmrSystem? EmrSystem { get; set; }
    public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public ICollection<Diagnosis> Diagnoses { get; set; } = new List<Diagnosis>();
    public ICollection<Allergy> Allergies { get; set; } = new List<Allergy>();
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
    public ICollection<InsurancePolicy> InsurancePolicies { get; set; } = new List<InsurancePolicy>();
    public ICollection<BillingInvoice> BillingInvoices { get; set; } = new List<BillingInvoice>();
    public Inpatient? Inpatient { get; set; }
    public Outpatient? Outpatient { get; set; }
    public EmergencyCase? EmergencyCase { get; set; }
}

public class Inpatient
{
    [Key, ForeignKey("PatientRecord")]
    public string RecordId { get; set; } = string.Empty;
    public DateOnly AdmitDate { get; set; }
    public DateOnly? DischargeDate { get; set; }
    public int Version { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public string? WardCode { get; set; }
    public Ward? Ward { get; set; }
    public string? BedNumber { get; set; }
    public Bed? Bed { get; set; }
    public PatientRecord? PatientRecord { get; set; }
}

public class Outpatient
{
    [Key, ForeignKey("PatientRecord")]
    public string RecordId { get; set; } = string.Empty;
    public DateOnly VisitDate { get; set; }
    public int Version { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public PatientRecord? PatientRecord { get; set; }
}

public class EmergencyCase
{
    [Key, ForeignKey("PatientRecord")]
    public string RecordId { get; set; } = string.Empty;
    public string TriageLevel { get; set; } = string.Empty;
    public DateTime ArrivalTime { get; set; }
    public int Version { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public PatientRecord? PatientRecord { get; set; }
}

public class Encounter
{
    [Key]
    public string EncounterId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
    public Guid TransactionId { get; set; }
    public string PatientRecordId { get; set; } = string.Empty;
    public PatientRecord? PatientRecord { get; set; }
    public string? PhysicianId { get; set; }
    public Physician? Physician { get; set; }
    public string? NurseLicense { get; set; }
    public Nurse? Nurse { get; set; }
    public ICollection<LabOrder> LabOrders { get; set; } = new List<LabOrder>();
    public ICollection<RadiologyOrder> RadiologyOrders { get; set; } = new List<RadiologyOrder>();
}

public class Appointment
{
    [Key]
    public string ApptId { get; set; } = string.Empty;
    public DateTime ScheduledTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public Guid TransactionId { get; set; }
    public string PatientRecordId { get; set; } = string.Empty;
    public PatientRecord? PatientRecord { get; set; }
    public string? PatientId { get; set; }
    public Patient? Patient { get; set; }
    public string? PhysicianId { get; set; }
    public Physician? Physician { get; set; }
}

public class Physician
{
    [Key]
    public string ProviderId { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    public Surgeon? Surgeon { get; set; }
}

public class Surgeon
{
    [Key, ForeignKey("Physician")]
    public string ProviderId { get; set; } = string.Empty;
    public bool BoardCertified { get; set; }
    public Physician? Physician { get; set; }
    public ICollection<SurgicalSchedule> SurgicalSchedules { get; set; } = new List<SurgicalSchedule>();
}

public class Nurse
{
    [Key]
    public string License { get; set; } = string.Empty;
    public ICollection<Encounter> Encounters { get; set; } = new List<Encounter>();
}

public class Ward
{
    [Key]
    public string WardCode { get; set; } = string.Empty;
    public Guid DepartmentId { get; set; }
    public Department? Department { get; set; }
    public ICollection<Bed> Beds { get; set; } = new List<Bed>();
}

public class Bed
{
    [Key]
    public string BedNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string WardCode { get; set; } = string.Empty;
    public Ward? Ward { get; set; }
    public ICollection<Inpatient> Inpatients { get; set; } = new List<Inpatient>();
}

public class SurgicalSchedule
{
    [Key]
    public string ScheduleId { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public Guid TransactionId { get; set; }
    public string? ProcedureCode { get; set; }
    public Procedure? Procedure { get; set; }
    public string? OperatingRoomNumber { get; set; }
    public OperatingRoom? OperatingRoom { get; set; }
    public string? SurgeonId { get; set; }
    public Surgeon? Surgeon { get; set; }
}

public class OperatingRoom
{
    [Key]
    public string OrNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public ICollection<SurgicalSchedule> SurgicalSchedules { get; set; } = new List<SurgicalSchedule>();
}

public class Procedure
{
    [Key]
    public string ProcCode { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ICollection<SurgicalSchedule> SurgicalSchedules { get; set; } = new List<SurgicalSchedule>();
    public ICollection<BillingInvoice> BillingInvoices { get; set; } = new List<BillingInvoice>();
}

public class LabOrder
{
    [Key]
    public string OrderId { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public Guid TransactionId { get; set; }
    public string EncounterId { get; set; } = string.Empty;
    public Encounter? Encounter { get; set; }
    public ICollection<LabResult> LabResults { get; set; } = new List<LabResult>();
}

public class LabResult
{
    [Key]
    public string ResultId { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Units { get; set; } = string.Empty;
    public int Version { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public string LabOrderId { get; set; } = string.Empty;
    public LabOrder? LabOrder { get; set; }
}

public class RadiologyOrder
{
    [Key]
    public string OrderId { get; set; } = string.Empty;
    public string Modality { get; set; } = string.Empty;
    public Guid TransactionId { get; set; }
    public string EncounterId { get; set; } = string.Empty;
    public Encounter? Encounter { get; set; }
    public ICollection<RadiologyImage> RadiologyImages { get; set; } = new List<RadiologyImage>();
}

public class RadiologyImage
{
    [Key]
    public string ImageId { get; set; } = string.Empty;
    public string Uri { get; set; } = string.Empty;
    public int Version { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public string RadiologyOrderId { get; set; } = string.Empty;
    public RadiologyOrder? RadiologyOrder { get; set; }
}

public class PharmacyInventory
{
    public Guid InventoryId { get; set; } = Guid.NewGuid();
    public DateOnly LastAudit { get; set; }
    public Guid EmrSystemId { get; set; }
    public EmrSystem? EmrSystem { get; set; }
    public ICollection<Medication> Medications { get; set; } = new List<Medication>();
}

public class Medication
{
    [Key]
    public string Ndc { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Stock { get; set; }
    public Guid PharmacyInventoryId { get; set; }
    public PharmacyInventory? PharmacyInventory { get; set; }
    public ICollection<Prescription> Prescriptions { get; set; } = new List<Prescription>();
}

public class Prescription
{
    [Key]
    public string RxNumber { get; set; } = string.Empty;
    public string Sig { get; set; } = string.Empty;
    public string PatientRecordId { get; set; } = string.Empty;
    public PatientRecord? PatientRecord { get; set; }
    public string? MedicationNdc { get; set; }
    public Medication? Medication { get; set; }
}

public class Allergy
{
    public Guid AllergyId { get; set; } = Guid.NewGuid();
    public string Substance { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string PatientRecordId { get; set; } = string.Empty;
    public PatientRecord? PatientRecord { get; set; }
}

public class Diagnosis
{
    public Guid DiagnosisId { get; set; } = Guid.NewGuid();
    public string Icd10 { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string PatientRecordId { get; set; } = string.Empty;
    public PatientRecord? PatientRecord { get; set; }
}

public class InsurancePolicy
{
    [Key]
    public string PolicyNumber { get; set; } = string.Empty;
    public string Payer { get; set; } = string.Empty;
    public string PatientRecordId { get; set; } = string.Empty;
    public PatientRecord? PatientRecord { get; set; }
    public ICollection<InsuranceClaim> InsuranceClaims { get; set; } = new List<InsuranceClaim>();
}

public class InsuranceClaim
{
    [Key]
    public string ClaimId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int Version { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public Guid TransactionId { get; set; }
    public string PolicyNumber { get; set; } = string.Empty;
    public InsurancePolicy? InsurancePolicy { get; set; }
}

public class BillingInvoice
{
    [Key]
    public string InvoiceId { get; set; } = string.Empty;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    public DateOnly DueDate { get; set; }
    public int Version { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public Guid TransactionId { get; set; }
    public string PatientRecordId { get; set; } = string.Empty;
    public PatientRecord? PatientRecord { get; set; }
    public string? PatientId { get; set; }
    public Patient? Patient { get; set; }
    public string? ProcedureCode { get; set; }
    public Procedure? Procedure { get; set; }
}
