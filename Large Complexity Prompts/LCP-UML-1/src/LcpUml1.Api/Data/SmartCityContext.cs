using LcpUml1.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LcpUml1.Api.Data;

public class SmartCityContext(DbContextOptions<SmartCityContext> options) : DbContext(options)
{
    public DbSet<VehicleDim> Vehicles => Set<VehicleDim>();
    public DbSet<RouteDim> Routes => Set<RouteDim>();
    public DbSet<Intersection> Intersections => Set<Intersection>();
    public DbSet<TrafficSensor> TrafficSensors => Set<TrafficSensor>();
    public DbSet<SignalController> SignalControllers => Set<SignalController>();
    public DbSet<GridPowerMonitor> GridPowerMonitors => Set<GridPowerMonitor>();
    public DbSet<MaintenanceSchedule> MaintenanceSchedules => Set<MaintenanceSchedule>();
    public DbSet<MaintenanceFact> MaintenanceFacts => Set<MaintenanceFact>();
    public DbSet<SignalEventFact> SignalEventFacts => Set<SignalEventFact>();
    public DbSet<PowerEventFact> PowerEventFacts => Set<PowerEventFact>();
    public DbSet<CalendarDim> Calendar => Set<CalendarDim>();
    public DbSet<SensorDim> SensorDimensions => Set<SensorDim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<VehicleDim>()
            .Property(v => v.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<SignalController>()
            .Property(v => v.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<TrafficSensor>()
            .Property(v => v.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<GridPowerMonitor>()
            .Property(v => v.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<MaintenanceSchedule>()
            .Property(v => v.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<PowerEventFact>()
            .Property(p => p.LoadKw)
            .HasPrecision(10, 2);

        modelBuilder.Entity<GridPowerMonitor>()
            .Property(p => p.CurrentLoadKw)
            .HasPrecision(10, 2);

        // Facts reference dimensions only -> 3NF layout
        modelBuilder.Entity<SignalEventFact>()
            .HasOne(f => f.SignalController)
            .WithMany(c => c.Events)
            .HasForeignKey(f => f.SignalControllerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<SignalEventFact>()
            .HasOne(f => f.Calendar)
            .WithMany()
            .HasForeignKey(f => f.DateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PowerEventFact>()
            .HasOne(f => f.GridPowerMonitor)
            .WithMany(p => p.PowerEvents)
            .HasForeignKey(f => f.GridPowerMonitorId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PowerEventFact>()
            .HasOne(f => f.Calendar)
            .WithMany()
            .HasForeignKey(f => f.DateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MaintenanceFact>()
            .HasOne(f => f.Vehicle)
            .WithMany()
            .HasForeignKey(f => f.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MaintenanceFact>()
            .HasOne(f => f.Schedule)
            .WithMany(s => s.MaintenanceFacts)
            .HasForeignKey(f => f.ScheduleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MaintenanceFact>()
            .HasOne(f => f.Calendar)
            .WithMany()
            .HasForeignKey(f => f.DateId)
            .OnDelete(DeleteBehavior.Restrict);

        base.OnModelCreating(modelBuilder);
    }
}
