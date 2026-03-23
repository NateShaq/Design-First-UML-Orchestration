using LCP.Vibe.Api.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/traffic/flow", () =>
{
    var sample = new TrafficFlowSnapshot
    {
        Corridor = "Downtown Loop",
        AverageSpeedMph = 28,
        VehicleCount = 1240,
        UpdatedAtUtc = DateTime.UtcNow
    };
    return Results.Ok(sample);
});

app.MapGet("/transit/routes", () =>
{
    var routes = new[]
    {
        new TransitRoute { Id = "LR-Blue", Mode = TransitMode.LightRail, Name = "Blue Line", HeadwayMinutes = 10 },
        new TransitRoute { Id = "EB-15", Mode = TransitMode.ElectricBus, Name = "15 Crosstown", HeadwayMinutes = 12 }
    };
    return Results.Ok(routes);
});

app.MapGet("/emergency/readiness", () =>
{
    var readiness = new EmergencyReadiness
    {
        ActiveUnits = 18,
        AverageResponseMinutes = 6.2m,
        HospitalsAtCapacity = 1,
        GeneratedAtUtc = DateTime.UtcNow
    };
    return Results.Ok(readiness);
});

app.Run();
