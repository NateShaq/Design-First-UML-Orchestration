using LcpUml3.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Data;

public class HospitalContext(DbContextOptions<HospitalContext> options) : DbContext(options)
{
    public DbSet<Hospital> Hospitals => Set<Hospital>();
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<EmrSystem> EmrSystems => Set<EmrSystem>();
    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<PatientRecord> PatientRecords => Set<PatientRecord>();
    public DbSet<Inpatient> Inpatients => Set<Inpatient>();
    public DbSet<Outpatient> Outpatients => Set<Outpatient>();
    public DbSet<EmergencyCase> EmergencyCases => Set<EmergencyCase>();
    public DbSet<Encounter> Encounters => Set<Encounter>();
    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Physician> Physicians => Set<Physician>();
    public DbSet<Surgeon> Surgeons => Set<Surgeon>();
    public DbSet<Nurse> Nurses => Set<Nurse>();
    public DbSet<Ward> Wards => Set<Ward>();
    public DbSet<Bed> Beds => Set<Bed>();
    public DbSet<SurgicalSchedule> SurgicalSchedules => Set<SurgicalSchedule>();
    public DbSet<OperatingRoom> OperatingRooms => Set<OperatingRoom>();
    public DbSet<Procedure> Procedures => Set<Procedure>();
    public DbSet<LabOrder> LabOrders => Set<LabOrder>();
    public DbSet<LabResult> LabResults => Set<LabResult>();
    public DbSet<RadiologyOrder> RadiologyOrders => Set<RadiologyOrder>();
    public DbSet<RadiologyImage> RadiologyImages => Set<RadiologyImage>();
    public DbSet<PharmacyInventory> PharmacyInventories => Set<PharmacyInventory>();
    public DbSet<Medication> Medications => Set<Medication>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();
    public DbSet<Allergy> Allergies => Set<Allergy>();
    public DbSet<Diagnosis> Diagnoses => Set<Diagnosis>();
    public DbSet<InsurancePolicy> InsurancePolicies => Set<InsurancePolicy>();
    public DbSet<InsuranceClaim> InsuranceClaims => Set<InsuranceClaim>();
    public DbSet<BillingInvoice> BillingInvoices => Set<BillingInvoice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Hospital aggregates
        modelBuilder.Entity<Department>()
            .HasOne(d => d.Hospital)
            .WithMany(h => h.Departments)
            .HasForeignKey(d => d.HospitalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EmrSystem>()
            .HasOne(e => e.Hospital)
            .WithMany(h => h.EmrSystems)
            .HasForeignKey(e => e.HospitalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ward>()
            .HasOne(w => w.Department)
            .WithMany(d => d.Wards)
            .HasForeignKey(w => w.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Bed>()
            .HasOne(b => b.Ward)
            .WithMany(w => w.Beds)
            .HasForeignKey(b => b.WardCode)
            .OnDelete(DeleteBehavior.Cascade);

        // Patient records and subtypes (table-per-type)
        modelBuilder.Entity<PatientRecord>()
            .HasOne(pr => pr.Patient)
            .WithMany(p => p.PatientRecords)
            .HasForeignKey(pr => pr.PatientId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PatientRecord>()
            .HasOne(pr => pr.EmrSystem)
            .WithMany(e => e.PatientRecords)
            .HasForeignKey(pr => pr.EmrSystemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Inpatient>().ToTable("Inpatients");
        modelBuilder.Entity<Outpatient>().ToTable("Outpatients");
        modelBuilder.Entity<EmergencyCase>().ToTable("EmergencyCases");

        modelBuilder.Entity<Inpatient>()
            .HasOne(i => i.PatientRecord)
            .WithOne(pr => pr.Inpatient)
            .HasForeignKey<Inpatient>(i => i.RecordId);

        modelBuilder.Entity<Inpatient>()
            .HasOne(i => i.Ward)
            .WithMany()
            .HasForeignKey(i => i.WardCode)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Inpatient>()
            .HasOne(i => i.Bed)
            .WithMany(b => b.Inpatients)
            .HasForeignKey(i => i.BedNumber)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Outpatient>()
            .HasOne(o => o.PatientRecord)
            .WithOne(pr => pr.Outpatient)
            .HasForeignKey<Outpatient>(o => o.RecordId);

        modelBuilder.Entity<EmergencyCase>()
            .HasOne(e => e.PatientRecord)
            .WithOne(pr => pr.EmergencyCase)
            .HasForeignKey<EmergencyCase>(e => e.RecordId);

        // Encounters & appointments
        modelBuilder.Entity<Encounter>()
            .HasOne(e => e.PatientRecord)
            .WithMany(pr => pr.Encounters)
            .HasForeignKey(e => e.PatientRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Encounter>()
            .HasOne(e => e.Physician)
            .WithMany(p => p.Encounters)
            .HasForeignKey(e => e.PhysicianId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Encounter>()
            .HasOne(e => e.Nurse)
            .WithMany(n => n.Encounters)
            .HasForeignKey(e => e.NurseLicense)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.PatientRecord)
            .WithMany(pr => pr.Appointments)
            .HasForeignKey(a => a.PatientRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PatientId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Appointment>()
            .HasOne(a => a.Physician)
            .WithMany(p => p.Appointments)
            .HasForeignKey(a => a.PhysicianId)
            .OnDelete(DeleteBehavior.SetNull);

        // Clinical orders
        modelBuilder.Entity<LabOrder>()
            .HasOne(lo => lo.Encounter)
            .WithMany(e => e.LabOrders)
            .HasForeignKey(lo => lo.EncounterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LabResult>()
            .Property(lr => lr.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<LabResult>()
            .HasOne(lr => lr.LabOrder)
            .WithMany(lo => lo.LabResults)
            .HasForeignKey(lr => lr.LabOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RadiologyOrder>()
            .HasOne(ro => ro.Encounter)
            .WithMany(e => e.RadiologyOrders)
            .HasForeignKey(ro => ro.EncounterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RadiologyImage>()
            .Property(ri => ri.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<RadiologyImage>()
            .HasOne(ri => ri.RadiologyOrder)
            .WithMany(ro => ro.RadiologyImages)
            .HasForeignKey(ri => ri.RadiologyOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        // Pharmacy
        modelBuilder.Entity<PharmacyInventory>()
            .HasOne(pi => pi.EmrSystem)
            .WithMany(e => e.PharmacyInventories)
            .HasForeignKey(pi => pi.EmrSystemId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Medication>()
            .HasOne(m => m.PharmacyInventory)
            .WithMany(pi => pi.Medications)
            .HasForeignKey(m => m.PharmacyInventoryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.PatientRecord)
            .WithMany(pr => pr.Prescriptions)
            .HasForeignKey(p => p.PatientRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Prescription>()
            .HasOne(p => p.Medication)
            .WithMany(m => m.Prescriptions)
            .HasForeignKey(p => p.MedicationNdc)
            .OnDelete(DeleteBehavior.SetNull);

        // Allergies & diagnoses
        modelBuilder.Entity<Allergy>()
            .HasOne(a => a.PatientRecord)
            .WithMany(pr => pr.Allergies)
            .HasForeignKey(a => a.PatientRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Diagnosis>()
            .HasOne(d => d.PatientRecord)
            .WithMany(pr => pr.Diagnoses)
            .HasForeignKey(d => d.PatientRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // Insurance & billing
        modelBuilder.Entity<InsurancePolicy>()
            .HasOne(ip => ip.PatientRecord)
            .WithMany(pr => pr.InsurancePolicies)
            .HasForeignKey(ip => ip.PatientRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InsuranceClaim>()
            .Property(ic => ic.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<InsuranceClaim>()
            .HasOne(ic => ic.InsurancePolicy)
            .WithMany(ip => ip.InsuranceClaims)
            .HasForeignKey(ic => ic.PolicyNumber)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BillingInvoice>()
            .Property(bi => bi.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<BillingInvoice>()
            .HasOne(bi => bi.PatientRecord)
            .WithMany(pr => pr.BillingInvoices)
            .HasForeignKey(bi => bi.PatientRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BillingInvoice>()
            .HasOne(bi => bi.Patient)
            .WithMany(p => p.BillingInvoices)
            .HasForeignKey(bi => bi.PatientId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<BillingInvoice>()
            .HasOne(bi => bi.Procedure)
            .WithMany(p => p.BillingInvoices)
            .HasForeignKey(bi => bi.ProcedureCode)
            .OnDelete(DeleteBehavior.SetNull);

        // Surgery
        modelBuilder.Entity<Surgeon>().ToTable("Surgeons");

        modelBuilder.Entity<Surgeon>()
            .HasOne(s => s.Physician)
            .WithOne(p => p.Surgeon)
            .HasForeignKey<Surgeon>(s => s.ProviderId);

        modelBuilder.Entity<SurgicalSchedule>()
            .HasOne(ss => ss.Procedure)
            .WithMany(p => p.SurgicalSchedules)
            .HasForeignKey(ss => ss.ProcedureCode)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<SurgicalSchedule>()
            .HasOne(ss => ss.OperatingRoom)
            .WithMany(or => or.SurgicalSchedules)
            .HasForeignKey(ss => ss.OperatingRoomNumber)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<SurgicalSchedule>()
            .HasOne(ss => ss.Surgeon)
            .WithMany(s => s.SurgicalSchedules)
            .HasForeignKey(ss => ss.SurgeonId)
            .OnDelete(DeleteBehavior.SetNull);

        // Column precision
        modelBuilder.Entity<BillingInvoice>()
            .Property(bi => bi.Amount)
            .HasColumnType("decimal(18,2)");

        // DateOnly mapping
        modelBuilder.Entity<Patient>()
            .Property(p => p.Dob)
            .HasColumnType("date");

        modelBuilder.Entity<Inpatient>()
            .Property(i => i.AdmitDate)
            .HasColumnType("date");

        modelBuilder.Entity<Inpatient>()
            .Property(i => i.DischargeDate)
            .HasColumnType("date");

        modelBuilder.Entity<Outpatient>()
            .Property(o => o.VisitDate)
            .HasColumnType("date");

        modelBuilder.Entity<SurgicalSchedule>()
            .Property(s => s.Date)
            .HasColumnType("date");

        modelBuilder.Entity<PharmacyInventory>()
            .Property(p => p.LastAudit)
            .HasColumnType("date");

        modelBuilder.Entity<BillingInvoice>()
            .Property(b => b.DueDate)
            .HasColumnType("date");
    }
}
