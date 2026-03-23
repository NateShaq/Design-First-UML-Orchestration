using AutonomousFleet.Api.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// In-memory demo stores
var telematicsFeed = new List<TelematicsReading>
{
    new() { VehicleId = "SED-1001", SpeedKph = 42, BatteryPercent = 82, Latitude = 41.8781, Longitude = -87.6298, Timestamp = DateTimeOffset.UtcNow },
    new() { VehicleId = "VAN-2001", SpeedKph = 15, BatteryPercent = 56, Latitude = 34.0522, Longitude = -118.2437, Timestamp = DateTimeOffset.UtcNow }
};

var bookings = new List<PassengerBooking>
{
    new()
    {
        Id = Guid.NewGuid(), Passenger = new Passenger { Id = Guid.NewGuid(), Name = "Avery Jones", Phone = "+1-555-0145" },
        PickupLat = 37.7749, PickupLng = -122.4194, DropoffLat = 37.3382, DropoffLng = -121.8863,
        Status = "Scheduled", ScheduledFor = DateTimeOffset.UtcNow.AddMinutes(30)
    }
};

var operatorSessions = new List<RemoteOperatorSupportSession>
{
    new() { Id = Guid.NewGuid(), Operator = new RemoteOperator { Id = Guid.NewGuid(), Name = "R. Patel", CertificationLevel = "L3" }, VehicleId = "DRN-3001", StartedAt = DateTimeOffset.UtcNow.AddMinutes(-5), Status = "Monitoring" }
};

// Vehicle telematics endpoints
app.MapGet("/api/telematics", () => Results.Ok(telematicsFeed));
app.MapPost("/api/telematics", (TelematicsReading reading) =>
{
    telematicsFeed.Add(reading with { Timestamp = DateTimeOffset.UtcNow });
    return Results.Created($"/api/telematics/{reading.VehicleId}", reading);
});

// Passenger booking endpoints
app.MapGet("/api/bookings", () => Results.Ok(bookings));
app.MapPost("/api/bookings", (PassengerBooking booking) =>
{
    booking.Id = Guid.NewGuid();
    booking.Status = "Scheduled";
    booking.ScheduledFor = booking.ScheduledFor == default ? DateTimeOffset.UtcNow.AddMinutes(10) : booking.ScheduledFor;
    bookings.Add(booking);
    return Results.Created($"/api/bookings/{booking.Id}", booking);
});
app.MapPost("/api/bookings/{id:guid}/cancel", (Guid id) =>
{
    var match = bookings.FirstOrDefault(b => b.Id == id);
    if (match is null) return Results.NotFound();
    match.Status = "Canceled";
    return Results.Ok(match);
});

// Remote operator support endpoints
app.MapGet("/api/operator-sessions", () => Results.Ok(operatorSessions));
app.MapPost("/api/operator-sessions", (RemoteOperatorSupportSession session) =>
{
    session.Id = Guid.NewGuid();
    session.StartedAt = DateTimeOffset.UtcNow;
    session.Status = "Active";
    operatorSessions.Add(session);
    return Results.Created($"/api/operator-sessions/{session.Id}", session);
});
app.MapPost("/api/operator-sessions/{id:guid}/handoff", (Guid id, RemoteAssistanceRequest request) =>
{
    var session = operatorSessions.FirstOrDefault(s => s.Id == id);
    if (session is null) return Results.NotFound();
    session.Status = "HandedOff";
    return Results.Ok(new { session.Id, session.VehicleId, request.Reason, request.Priority });
});

app.MapGet("/", () => Results.Ok(new
{
    Service = "Autonomous Vehicle Fleet Network",
    Domains = new[] { "VehicleTelematics", "PassengerBooking", "RemoteOperatorSupport" },
    Timestamp = DateTimeOffset.UtcNow
}));

app.Run();
