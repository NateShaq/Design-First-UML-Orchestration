using LcpUml6.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Data;

public class GridDbContext : DbContext
{
    public GridDbContext(DbContextOptions<GridDbContext> options) : base(options) { }

    public DbSet<SolarFarm> SolarFarms => Set<SolarFarm>();
    public DbSet<WindTurbine> WindTurbines => Set<WindTurbine>();
    public DbSet<HydroPlant> HydroPlants => Set<HydroPlant>();
    public DbSet<BatteryStorage> BatteryStorages => Set<BatteryStorage>();
    public DbSet<SubstationMonitor> SubstationMonitors => Set<SubstationMonitor>();
    public DbSet<LoadBalancer> LoadBalancers => Set<LoadBalancer>();
    public DbSet<SmartMeter> SmartMeters => Set<SmartMeter>();
    public DbSet<TariffPlan> TariffPlans => Set<TariffPlan>();
    public DbSet<UsageBilling> UsageBillings => Set<UsageBilling>();
    public DbSet<SettlementEngine> SettlementEngines => Set<SettlementEngine>();
    public DbSet<GridConnection> GridConnections => Set<GridConnection>();
    public DbSet<MaintenanceSchedule> MaintenanceSchedules => Set<MaintenanceSchedule>();
    public DbSet<WeatherStation> WeatherStations => Set<WeatherStation>();
    public DbSet<ForecastService> ForecastServices => Set<ForecastService>();
    public DbSet<Inverter> Inverters => Set<Inverter>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SolarFarm>(entity =>
        {
            entity.Property(e => e.AssetId).ValueGeneratedNever();
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.HasOne(e => e.GridConnection).WithMany().HasForeignKey(e => e.GridConnectionId);
            entity.HasOne(e => e.WeatherStation).WithMany().HasForeignKey(e => e.WeatherStationId);
            entity.HasOne(e => e.BatteryStorage).WithMany().HasForeignKey(e => e.BatteryStorageId);
            entity.HasOne(e => e.Inverter).WithMany().HasForeignKey(e => e.InverterId);
            entity.HasOne(e => e.MaintenanceSchedule).WithMany().HasForeignKey(e => e.MaintenanceScheduleId);
        });

        modelBuilder.Entity<WindTurbine>(entity =>
        {
            entity.Property(e => e.AssetId).ValueGeneratedNever();
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.HasOne(e => e.GridConnection).WithMany().HasForeignKey(e => e.GridConnectionId);
            entity.HasOne(e => e.MaintenanceSchedule).WithMany().HasForeignKey(e => e.MaintenanceScheduleId);
            entity.HasOne(e => e.Inverter).WithMany().HasForeignKey(e => e.InverterId);
        });

        modelBuilder.Entity<HydroPlant>(entity =>
        {
            entity.Property(e => e.AssetId).ValueGeneratedNever();
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.HasOne(e => e.GridConnection).WithMany().HasForeignKey(e => e.GridConnectionId);
            entity.HasOne(e => e.MaintenanceSchedule).WithMany().HasForeignKey(e => e.MaintenanceScheduleId);
        });

        modelBuilder.Entity<BatteryStorage>(entity =>
        {
            entity.Property(e => e.AssetId).ValueGeneratedNever();
            entity.Property(e => e.RowVersion).IsRowVersion();
        });

        modelBuilder.Entity<SubstationMonitor>(entity =>
        {
            entity.Property(e => e.MonitorId).ValueGeneratedNever();
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.HasOne(e => e.GridConnection).WithMany().HasForeignKey(e => e.GridConnectionId);
        });

        modelBuilder.Entity<LoadBalancer>(entity =>
        {
            entity.Property(e => e.BalancerId).ValueGeneratedNever();
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.HasOne(e => e.GridConnection).WithMany().HasForeignKey(e => e.GridConnectionId);
        });

        modelBuilder.Entity<SmartMeter>(entity =>
        {
            entity.Property(e => e.MeterId).ValueGeneratedNever();
            entity.HasOne(e => e.GridConnection).WithMany().HasForeignKey(e => e.GridConnectionId);
        });

        modelBuilder.Entity<TariffPlan>(entity =>
        {
            entity.Property(e => e.TariffPlanId).ValueGeneratedNever();
        });

        modelBuilder.Entity<SettlementEngine>(entity =>
        {
            entity.Property(e => e.SettlementId).ValueGeneratedNever();
        });

        modelBuilder.Entity<UsageBilling>(entity =>
        {
            entity.Property(e => e.BillingId).ValueGeneratedNever();
            entity.Property(e => e.RowVersion).IsRowVersion();
            entity.HasOne(e => e.Meter).WithMany().HasForeignKey(e => e.MeterId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.TariffPlan).WithMany().HasForeignKey(e => e.TariffPlanId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(e => e.Settlement).WithMany().HasForeignKey(e => e.SettlementId).OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<GridConnection>(entity =>
        {
            entity.Property(e => e.GridConnectionId).ValueGeneratedNever();
        });

        modelBuilder.Entity<MaintenanceSchedule>(entity =>
        {
            entity.Property(e => e.MaintenanceScheduleId).ValueGeneratedNever();
        });

        modelBuilder.Entity<WeatherStation>(entity =>
        {
            entity.Property(e => e.WeatherStationId).ValueGeneratedNever();
        });

        modelBuilder.Entity<ForecastService>(entity =>
        {
            entity.Property(e => e.ForecastServiceId).ValueGeneratedNever();
            entity.HasOne(e => e.WeatherStation).WithMany().HasForeignKey(e => e.WeatherStationId);
        });

        modelBuilder.Entity<Inverter>(entity =>
        {
            entity.Property(e => e.InverterId).ValueGeneratedNever();
        });
    }
}
