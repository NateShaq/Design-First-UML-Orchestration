# LCP-Vibe-4 Autonomous Vehicle Fleet Network

This stub .NET 8 Web API sketches an autonomous vehicle fleet network with Vehicle Telematics, Passenger Booking, and Remote Operator Support domains. It includes 30 domain classes plus a starter SQL schema for persistence.

## Structure
- `src/AutonomousFleet.Api/Program.cs` – minimal API endpoints for telematics, bookings, and operator support.
- `src/AutonomousFleet.Api/Domain/Entities/` – 30 POCO classes modeling vehicles, sensors, bookings, and operations.
- `database/schema.sql` – relational schema covering key aggregates.

## Getting Started
1. Install .NET 8 SDK.
2. From `src/AutonomousFleet.Api`, run `dotnet restore` then `dotnet run` (project file included).
3. Endpoints are stubbed and return sample payloads; wire to a database using the schema as a starting point.

## Notes
- Telemetry and incidents are modeled for quick prototyping; add persistence/validation as needed.
- Replace in-memory lists with repositories/EF Core when integrating.
