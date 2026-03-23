using HospitalApi.Data;
using HospitalApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddSingleton<HospitalRepository>();
builder.Services.AddSingleton<PatientService>();
builder.Services.AddSingleton<PharmacyService>();
builder.Services.AddSingleton<SurgeryService>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.MapGet("/health", () => Results.Ok(new { status = "ok" }));

app.Run();
