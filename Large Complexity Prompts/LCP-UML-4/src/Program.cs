using FleetNetworkApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FleetDbContext>(options =>
{
    var conn = builder.Configuration.GetConnectionString("FleetDatabase")
               ?? "Server=localhost;Database=FleetNetworkDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;";
    options.UseSqlServer(conn);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
