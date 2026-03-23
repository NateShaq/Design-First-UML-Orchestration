using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using LCP.Vibe8.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<PlmRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/raw-materials", (PlmRepository repo) => repo.RawMaterials);
app.MapGet("/api/sub-assemblies", (PlmRepository repo) => repo.SubAssemblies);
app.MapGet("/api/finished-products", (PlmRepository repo) => repo.FinishedProducts);
app.MapGet("/api/work-orders", (PlmRepository repo) => repo.WorkOrders);
app.MapGet("/api/inspection-results", (PlmRepository repo) => repo.InspectionResults);
app.MapPost("/api/change-orders", (PlmRepository repo, ChangeOrder change) => repo.AddChangeOrder(change));

app.MapGet("/api/health", () => new { status = "ok", timestamp = DateTime.UtcNow });

app.Run();
