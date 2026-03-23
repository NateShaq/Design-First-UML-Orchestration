using FleetNetworkApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FleetNetworkApi.Data;

public class FleetDbContext : DbContext
{
    public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options) { }

    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<RoutePlan> RoutePlans => Set<RoutePlan>();
    public DbSet<PassengerProfile> Passengers => Set<PassengerProfile>();
    public DbSet<PaymentAccount> PaymentAccounts => Set<PaymentAccount>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<TripAssignment> TripAssignments => Set<TripAssignment>();
    public DbSet<MaintenanceSchedule> MaintenanceSchedules => Set<MaintenanceSchedule>();
    public DbSet<IncidentReport> IncidentReports => Set<IncidentReport>();
    public DbSet<ChargingStation> ChargingStations => Set<ChargingStation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 3NF: enforce unique VIN and proper FK relations
        modelBuilder.Entity<Vehicle>()
            .HasIndex(v => v.Vin)
            .IsUnique();

        modelBuilder.Entity<RoutePlan>()
            .HasOne(r => r.Vehicle)
            .WithOne(v => v.ActiveRoute)
            .HasForeignKey<RoutePlan>(r => r.VehicleId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Passenger)
            .WithMany()
            .HasForeignKey(b => b.PassengerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.PaymentAccount)
            .WithMany()
            .HasForeignKey(b => b.PaymentAccountId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TripAssignment>()
            .HasOne(t => t.Booking)
            .WithMany()
            .HasForeignKey(t => t.BookingId);

        modelBuilder.Entity<TripAssignment>()
            .HasOne(t => t.Vehicle)
            .WithMany(v => v.TripAssignments)
            .HasForeignKey(t => t.VehicleId);

        modelBuilder.Entity<MaintenanceSchedule>()
            .HasOne(m => m.Vehicle)
            .WithMany(v => v.MaintenanceSchedules)
            .HasForeignKey(m => m.VehicleId);

        modelBuilder.Entity<IncidentReport>()
            .HasOne(i => i.Vehicle)
            .WithMany(v => v.IncidentReports)
            .HasForeignKey(i => i.VehicleId);

        modelBuilder.Entity<IncidentReport>()
            .HasOne(i => i.RoutePlan)
            .WithMany()
            .HasForeignKey(i => i.RoutePlanId);

        modelBuilder.Entity<ChargingStation>()
            .HasOne(c => c.Vehicle)
            .WithMany()
            .HasForeignKey(c => c.VehicleId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
