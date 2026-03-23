# LCP-UML-7 Web API

C# .NET 8 Web API implementing the validated UML for the Global Higher Education ERP. Persistence uses SQL Server with 3NF tables, optimistic concurrency via `[Timestamp]` rowversion, and serializable isolation on critical workflows.

## Getting started
1. `cd LCP-UML-7/src/LCP.Uml7.Api`
2. Update `appsettings.json` connection string `SqlServer`.
3. Restore & create initial migration:
   ```bash
   dotnet restore
   dotnet ef migrations add InitialCreate
   dotnet ef database update
   dotnet run
   ```

## Concurrency & transaction rules
- ThreadSafe entities (`Student` hierarchy, `ResearchFellow`, `CourseCurriculum`, `FinancialAidPackage`, `Transcript`, `FacultyGrant`) carry `[Timestamp] RowVersion` for ghost-write protection.
- Critical controllers (`CourseOfferingsController`, `EnrollmentsController`, `BillingController`) wrap writes in `IsolationLevel.Serializable` transactions to maintain ACID semantics from the UML begin/commit/rollback contracts.

## Key endpoints
- `POST /api/students/undergraduates` / `.../postgraduates` create with revision tracking.
- `PUT /api/students/{id}` uses RowVersion for optimistic concurrency.
- `POST /api/course-offerings` create offerings; `PUT /api/course-offerings/{id}/capacity` capacity guard.
- `POST /api/enrollments` enforces capacity with serializable isolation.
- `POST /api/billing/accounts` create accounts, `POST /api/billing/accounts/{id}/invoices`, `POST /api/billing/invoices/{id}/payments` adjust balances atomically.

## Schema notes (3NF)
- All many-to-many associations are decomposed into join tables (e.g., `StudentCourseOffering`, `ResearchProjectCourseOffering`, `AlumniEvent`).
- Composition/aggregation from the UML is modeled with required foreign keys; optional associations use nullable FK with `SetNull` deletes.
- Monetary values use `decimal(18,2)`; keys are UUID (GUID) per UML surrogate key guidance.
