# LCP-UML-1 Smart City Web API

ASP.NET Core 8 Web API and SQL Server schema derived strictly from `LCP-UML-1.puml` (Traffic Flow, Public Transit, Emergency Response, Platform/ACID, 3NF facts/dims).

## Project layout
- `LcpUml1.Api/` – API project with EF Core entities, DbContext, and controllers.
- `database/schema.sql` – Handwritten SQL Server DDL matching the PlantUML 3NF model with `ROWVERSION` ghost-write guards.

## Key design choices
- **Ghost-write protection**: `[Timestamp] byte[] RowVersion` on VersionedResource aggregates (vehicles, sensors, signal controllers, grid power monitors, maintenance schedules). EF maps to SQL `rowversion`. Updates set `OriginalValue` so stale writes fail with HTTP 409.
- **Isolation**: POST/PUT/DELETE in critical controllers run inside `IsolationLevel.Serializable` transactions to guard high-contention write paths.
- **3NF**: Fact tables reference only dimension keys (`SignalEventFact`, `PowerEventFact`, `MaintenanceFact`) with strict FK constraints and no transitive dependencies.
- **Precision**: Power load uses `decimal(10,2)`; dates use `DateOnly` in code and `date` in SQL.

## Running locally
1) Ensure SQL Server/LocalDB is available; adjust `ConnectionStrings:SmartCityDb` in `appsettings*.json` if needed.  
2) Restore and build:
```bash
cd LCP-UML-1/src/LcpUml1.Api
dotnet restore
dotnet run
```
3) Apply schema (if not using EF migrations) to your SQL Server instance:
```bash
sqlcmd -S (localdb)\\MSSQLLocalDB -d LcpUml1Db -i ../database/schema.sql
```
4) Browse Swagger UI at `http://localhost:5000/swagger`.

## API hints
- `RowVersion` appears as base64 in JSON. Include the returned value on subsequent updates to avoid 409 conflicts.
- Controllers covering ghost-write-prone aggregates: `Vehicles`, `SignalControllers`, `TrafficSensors`, `GridPowerMonitors`, `MaintenanceSchedules`.
- Fact ingestion endpoints live under `api/facts/*` and keep write paths lean (no serializable transaction needed for append-only facts).
