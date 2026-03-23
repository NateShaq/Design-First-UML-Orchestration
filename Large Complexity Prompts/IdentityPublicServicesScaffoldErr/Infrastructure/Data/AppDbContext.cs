using IdentityPublicServices.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdentityPublicServices.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<IdentityRegistry> IdentityRegistries => Set<IdentityRegistry>();
    public DbSet<Person> People => Set<Person>();
    public DbSet<Citizen> Citizens => Set<Citizen>();
    public DbSet<Resident> Residents => Set<Resident>();
    public DbSet<ForeignNational> ForeignNationals => Set<ForeignNational>();
    public DbSet<BiometricData> BiometricData => Set<BiometricData>();
    public DbSet<AuditTrail> AuditTrails => Set<AuditTrail>();
    public DbSet<Jurisdiction> Jurisdictions => Set<Jurisdiction>();
    public DbSet<PublicRecord> PublicRecords => Set<PublicRecord>();
    public DbSet<DataRetentionPolicy> DataRetentionPolicies => Set<DataRetentionPolicy>();
    public DbSet<ConsentRecord> ConsentRecords => Set<ConsentRecord>();

    public DbSet<IdentityProvider> IdentityProviders => Set<IdentityProvider>();
    public DbSet<AuthenticationService> AuthenticationServices => Set<AuthenticationService>();
    public DbSet<AuthorizationService> AuthorizationServices => Set<AuthorizationService>();
    public DbSet<DigitalWallet> DigitalWallets => Set<DigitalWallet>();
    public DbSet<Credential> Credentials => Set<Credential>();
    public DbSet<ServicePortal> ServicePortals => Set<ServicePortal>();
    public DbSet<NotificationService> NotificationServices => Set<NotificationService>();
    public DbSet<VerificationRequest> VerificationRequests => Set<VerificationRequest>();
    public DbSet<VerificationResponse> VerificationResponses => Set<VerificationResponse>();

    public DbSet<Agency> Agencies => Set<Agency>();
    public DbSet<MunicipalService> MunicipalServices => Set<MunicipalService>();
    public DbSet<NationalService> NationalServices => Set<NationalService>();
    public DbSet<TaxCollection> TaxCollections => Set<TaxCollection>();
    public DbSet<SocialSecurityBenefits> SocialSecurityBenefits => Set<SocialSecurityBenefits>();
    public DbSet<PassportService> PassportServices => Set<PassportService>();
    public DbSet<DriversLicense> DriversLicenses => Set<DriversLicense>();
    public DbSet<VotingRegistry> VotingRegistries => Set<VotingRegistry>();
    public DbSet<HealthServices> HealthServices => Set<HealthServices>();
    public DbSet<EducationServices> EducationServices => Set<EducationServices>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Person inheritance (TPH)
        modelBuilder.Entity<Person>()
            .HasDiscriminator<string>("PersonType")
            .HasValue<Person>("Person")
            .HasValue<Citizen>("Citizen")
            .HasValue<Resident>("Resident")
            .HasValue<ForeignNational>("ForeignNational");

        // Agency inheritance (TPH)
        modelBuilder.Entity<Agency>()
            .HasDiscriminator<string>("AgencyType")
            .HasValue<MunicipalService>("MunicipalService")
            .HasValue<NationalService>("NationalService");

        modelBuilder.Entity<IdentityRegistry>()
            .HasOne(ir => ir.Person)
            .WithMany(p => p.IdentityRegistries)
            .HasForeignKey(ir => ir.PersonId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<IdentityRegistry>()
            .HasOne(ir => ir.Jurisdiction)
            .WithMany(j => j.IdentityRegistries)
            .HasForeignKey(ir => ir.JurisdictionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IdentityRegistry>()
            .HasOne(ir => ir.IdentityProvider)
            .WithMany(ip => ip.IdentityRegistries)
            .HasForeignKey(ir => ir.IdentityProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<BiometricData>()
            .Property(b => b.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<BiometricData>()
            .HasOne(b => b.IdentityRegistry)
            .WithMany(ir => ir.BiometricData)
            .HasForeignKey(b => b.IdentityRegistryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<BiometricData>()
            .HasOne(b => b.ConsentRecord)
            .WithMany(c => c.BiometricData)
            .HasForeignKey(b => b.ConsentRecordId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuditTrail>()
            .Property(a => a.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<AuditTrail>()
            .HasOne(a => a.IdentityRegistry)
            .WithMany(ir => ir.AuditTrails)
            .HasForeignKey(a => a.IdentityRegistryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AuditTrail>()
            .HasOne(a => a.DataRetentionPolicy)
            .WithMany(d => d.AuditTrails)
            .HasForeignKey(a => a.DataRetentionPolicyId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Jurisdiction>()
            .Property(j => j.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<PublicRecord>()
            .Property(pr => pr.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<PublicRecord>()
            .HasOne(pr => pr.IdentityRegistry)
            .WithMany(ir => ir.PublicRecords)
            .HasForeignKey(pr => pr.IdentityRegistryId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PublicRecord>()
            .HasOne(pr => pr.Jurisdiction)
            .WithMany(j => j.PublicRecords)
            .HasForeignKey(pr => pr.JurisdictionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<IdentityProvider>()
            .HasMany(ip => ip.AuthenticationServices)
            .WithOne(a => a.IdentityProvider)
            .HasForeignKey(a => a.IdentityProviderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AuthenticationService>()
            .HasOne(a => a.AuthorizationService)
            .WithOne(auth => auth.AuthenticationService)
            .HasForeignKey<AuthorizationService>(auth => auth.AuthenticationServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AuthenticationService>()
            .HasOne(a => a.ServicePortal)
            .WithOne(sp => sp.AuthenticationService!)
            .HasForeignKey<ServicePortal>(sp => sp.AuthenticationServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuthorizationService>()
            .HasOne(a => a.ServicePortal)
            .WithOne(sp => sp.AuthorizationService!)
            .HasForeignKey<ServicePortal>(sp => sp.AuthorizationServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AuthorizationService>()
            .HasOne(a => a.ConsentRecord)
            .WithMany()
            .HasForeignKey(a => a.ConsentRecordId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServicePortal>()
            .HasMany(sp => sp.DigitalWallets)
            .WithOne(dw => dw.ServicePortal)
            .HasForeignKey(dw => dw.ServicePortalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServicePortal>()
            .HasOne(sp => sp.NotificationService)
            .WithOne(ns => ns.ServicePortal)
            .HasForeignKey<NotificationService>(ns => ns.ServicePortalId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DigitalWallet>()
            .HasMany(dw => dw.Credentials)
            .WithOne(c => c.DigitalWallet)
            .HasForeignKey(c => c.DigitalWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Credential>()
            .HasOne(c => c.PassportService)
            .WithOne(ps => ps.Credential!)
            .HasForeignKey<PassportService>(ps => ps.CredentialId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Credential>()
            .HasOne(c => c.DriversLicense)
            .WithOne(dl => dl.Credential)
            .HasForeignKey<DriversLicense>(dl => dl.CredentialId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VerificationRequest>()
            .HasOne(vr => vr.IdentityRegistry)
            .WithMany()
            .HasForeignKey(vr => vr.IdentityRegistryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VerificationRequest>()
            .HasOne(vr => vr.IdentityProvider)
            .WithMany(ip => ip.VerificationRequests)
            .HasForeignKey(vr => vr.IdentityProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<VerificationResponse>()
            .HasOne(vr => vr.VerificationRequest)
            .WithMany(req => req.Responses)
            .HasForeignKey(vr => vr.VerificationRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaxCollection>()
            .HasOne(tc => tc.PublicRecord)
            .WithMany()
            .HasForeignKey(tc => tc.PublicRecordId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TaxCollection>()
            .HasOne(tc => tc.NationalService)
            .WithMany(ns => ns.TaxCollections)
            .HasForeignKey(tc => tc.NationalServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SocialSecurityBenefits>()
            .HasOne(ssb => ssb.NationalService)
            .WithMany(ns => ns.SocialSecurityBenefits)
            .HasForeignKey(ssb => ssb.NationalServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<SocialSecurityBenefits>()
            .HasOne(ssb => ssb.PublicRecord)
            .WithMany()
            .HasForeignKey(ssb => ssb.PublicRecordId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HealthServices>()
            .HasOne(h => h.NationalService)
            .WithMany(ns => ns.HealthServices)
            .HasForeignKey(h => h.NationalServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<EducationServices>()
            .HasOne(e => e.NationalService)
            .WithMany(ns => ns.EducationServices)
            .HasForeignKey(e => e.NationalServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PassportService>()
            .Property(ps => ps.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<PassportService>()
            .HasOne(ps => ps.NationalService)
            .WithMany(ns => ns.PassportServices)
            .HasForeignKey(ps => ps.NationalServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DriversLicense>()
            .Property(dl => dl.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<DriversLicense>()
            .HasOne(dl => dl.MunicipalService)
            .WithMany(ms => ms.DriversLicenses)
            .HasForeignKey(dl => dl.MunicipalServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VotingRegistry>()
            .Property(v => v.RowVersion)
            .IsRowVersion();

        modelBuilder.Entity<VotingRegistry>()
            .HasOne(v => v.MunicipalService)
            .WithMany(ms => ms.VotingRegistries)
            .HasForeignKey(v => v.MunicipalServiceId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VotingRegistry>()
            .HasOne(v => v.PublicRecord)
            .WithMany()
            .HasForeignKey(v => v.PublicRecordId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
