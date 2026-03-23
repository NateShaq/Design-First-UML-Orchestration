# Smart City Traffic & Infrastructure Grid (LCP-Vibe-1)

Minimal .NET 8 Web API plus relational schema covering three pillars:

- TrafficFlow: live congestion, sensors, signals.
- PublicTransit: electric buses, light rail, autonomous shuttles.
- EmergencyResponse: dispatch, units, readiness metrics.
- Infrastructure/Grid: power + network backbone for the above.

## Layout
- `Program.cs` — minimal API with sample endpoints for each pillar.
- `Models/` — 42 POCOs (≥30 required) grouped by domain.
- `schema.sql` — PostgreSQL-friendly DDL for core entities.
- `LCP.Vibe.Api.csproj` — project file targeting `net8.0`.

## Quick start
```
cd LCP-Vibe-1
dotnet run
```
Then visit Swagger UI at `https://localhost:5001/swagger` (dev profile).

## Extending
- Add EF Core or Dapper as needed; map models to the tables in `schema.sql`.
- Expand endpoints by adding controllers or minimal API routes for each model.
- Wire telemetry: sensors/monitors feed a message broker, API persists to DB.
