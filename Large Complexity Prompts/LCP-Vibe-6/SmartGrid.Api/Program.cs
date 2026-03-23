using SmartGrid.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemoryDatabase>();
builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();

app.Run();
