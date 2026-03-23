# LCP-UML-3 Hospital EMR Web API

ASP.NET Core 8 Web API + SQL Server schema derived strictly from `LCP-UML-3.puml` (comprehensive hospital EMR). Built for 3NF, optimistic concurrency on ghost-write-prone aggregates, and serializable transactions on critical controllers.

## Structure
- `LcpUml3.Api/` – API project with EF Core entities matching the UML, `HospitalContext`, and controllers.
- `database/schema.sql` – hand-authored T-SQL DDL (3NF) with `rowversion` for ghost-write protection.

## Concurrency & Integrity
- `[Timestamp] byte[] RowVersion` on Inpatient, Outpatient, EmergencyCase, LabResult, RadiologyImage, InsuranceClaim, BillingInvoice. Updates set `OriginalValue`; stale writes return HTTP 409.
- Controllers for the above aggregates wrap POST/PUT/DELETE in `IsolationLevel.Serializable` transactions.
- 3NF: every table has a single key and only atomic attributes; references are via FK only (no transitive dependencies between clinical, patient, and financial domains).

## Running locally
1. Adjust `ConnectionStrings:HospitalDb` in `LcpUml3.Api/appsettings*.json` for your SQL Server/LocalDB.
2. Apply schema (no migrations needed):
   ```bash
   sqlcmd -S (localdb)\\MSSQLLocalDB -d LcpUml3Db -i database/schema.sql
   ```
3. Restore & run API (NuGet required):
   ```bash
   cd LcpUml3.Api
   dotnet restore
   dotnet run
   ```
4. Browse Swagger at `http://localhost:5000/swagger` (development profile).

## Key endpoints (all under `/api/...`)
- Base data: `Patients`, `PatientRecords`, `LabOrders`, `RadiologyOrders`.
- Ghost-write protected + serializable: `Inpatients`, `Outpatients`, `EmergencyCases`, `LabResults`, `RadiologyImages`, `InsuranceClaims`, `BillingInvoices`.

## Notes
- Date fields map to SQL `date`; money uses `decimal(18,2)`.
- Surgeon uses TPT inheritance on Physician; PatientRecord has TPT subtypes for Inpatient/Outpatient/EmergencyCase.
- Pharmacy inventory is scoped to an EMR system as in the UML composition.
