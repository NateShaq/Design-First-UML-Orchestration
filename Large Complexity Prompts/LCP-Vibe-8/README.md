# LCP-Vibe-8 PLM API (scaffold)

A minimal C# (.NET 8-style) Web API stub plus SQL schema for a multi-national manufacturing PLM covering Engineering Design, Shop Floor Execution, and Quality Assurance.

## Layout
- `src/LCP.Vibe8.Api/Program.cs` – minimal API endpoints and Swagger.
- `src/LCP.Vibe8.Api/Models/DomainModels.cs` – 30 domain classes (RawMaterial through TraceabilityRecord).
- `src/LCP.Vibe8.Api/Data/PlmRepository.cs` – in-memory repository with seed data and POST for change orders.
- `db/schema.sql` – starter SQL DDL for all 30 tables.

## Running (locally with .NET 8 SDK)
```bash
cd src/LCP.Vibe8.Api
dotnet restore # if you add a csproj
dotnet run
```
Swagger UI: `http://localhost:5000/swagger`

## Next steps
- Add a `.csproj` and EF Core DbContext if you want persistence.
- Map additional endpoints for QA, design, and shop-floor flows.
- Wire foreign keys in `db/schema.sql` to match your chosen data relationships.
