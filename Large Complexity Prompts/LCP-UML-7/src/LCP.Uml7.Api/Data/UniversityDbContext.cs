using LCP.Uml7.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace LCP.Uml7.Api.Data;

public class UniversityDbContext : DbContext
{
    public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options)
    {
    }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Undergraduate> Undergraduates => Set<Undergraduate>();
    public DbSet<Postgraduate> Postgraduates => Set<Postgraduate>();
    public DbSet<AcademicStaff> AcademicStaff => Set<AcademicStaff>();
    public DbSet<Faculty> FacultyMembers => Set<Faculty>();
    public DbSet<ResearchFellow> ResearchFellows => Set<ResearchFellow>();
    public DbSet<AdmissionsOfficer> AdmissionsOfficers => Set<AdmissionsOfficer>();
    public DbSet<Registrar> Registrars => Set<Registrar>();
    public DbSet<Alumni> Alumni => Set<Alumni>();
    public DbSet<CourseCurriculum> CourseCurricula => Set<CourseCurriculum>();
    public DbSet<CourseModule> CourseModules => Set<CourseModule>();
    public DbSet<CourseOffering> CourseOfferings => Set<CourseOffering>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();
    public DbSet<Transcript> Transcripts => Set<Transcript>();
    public DbSet<GradeEntry> GradeEntries => Set<GradeEntry>();
    public DbSet<LearningManagementSystem> LearningManagementSystems => Set<LearningManagementSystem>();
    public DbSet<OnlineCourse> OnlineCourses => Set<OnlineCourse>();
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<Submission> Submissions => Set<Submission>();
    public DbSet<FinancialAidPackage> FinancialAidPackages => Set<FinancialAidPackage>();
    public DbSet<Scholarship> Scholarships => Set<Scholarship>();
    public DbSet<LoanAid> LoanAids => Set<LoanAid>();
    public DbSet<PaymentPlan> PaymentPlans => Set<PaymentPlan>();
    public DbSet<BillingAccount> BillingAccounts => Set<BillingAccount>();
    public DbSet<TuitionInvoice> TuitionInvoices => Set<TuitionInvoice>();
    public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
    public DbSet<ResearchProject> ResearchProjects => Set<ResearchProject>();
    public DbSet<FacultyGrant> FacultyGrants => Set<FacultyGrant>();
    public DbSet<AlumniRelations> AlumniRelations => Set<AlumniRelations>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<AdmissionsOfficerEnrollment> AdmissionsOfficerEnrollments => Set<AdmissionsOfficerEnrollment>();
    public DbSet<RegistrarTranscript> RegistrarTranscripts => Set<RegistrarTranscript>();
    public DbSet<StudentCourseOffering> StudentCourseOfferings => Set<StudentCourseOffering>();
    public DbSet<ResearchProjectCourseOffering> ResearchProjectCourseOfferings => Set<ResearchProjectCourseOffering>();
    public DbSet<AlumniEvent> AlumniEvents => Set<AlumniEvent>();
    public DbSet<AlumniRelationsAlumni> AlumniRelationsAlumni => Set<AlumniRelationsAlumni>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>()
            .HasDiscriminator<string>("StudentType")
            .HasValue<Undergraduate>("Undergraduate")
            .HasValue<Postgraduate>("Postgraduate");

        modelBuilder.Entity<AcademicStaff>()
            .HasDiscriminator<string>("StaffType")
            .HasValue<Faculty>("Faculty")
            .HasValue<ResearchFellow>("ResearchFellow");

        modelBuilder.Entity<FinancialAidPackage>()
            .HasDiscriminator<string>("AidType")
            .HasValue<Scholarship>("Scholarship")
            .HasValue<LoanAid>("LoanAid");

        modelBuilder.Entity<CourseModule>()
            .HasOne(m => m.Curriculum)
            .WithMany(c => c.Modules)
            .HasForeignKey(m => m.CurriculumId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CourseOffering>()
            .HasOne(o => o.Module)
            .WithMany(m => m.Offerings)
            .HasForeignKey(o => o.ModuleId);

        modelBuilder.Entity<CourseOffering>()
            .HasOne(o => o.Faculty)
            .WithMany(f => f.CourseOfferings)
            .HasForeignKey(o => o.FacultyId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Offering)
            .WithMany(o => o.Enrollments)
            .HasForeignKey(e => e.OfferingId);

        modelBuilder.Entity<Transcript>()
            .HasOne(t => t.Student)
            .WithMany(s => s.Transcripts)
            .HasForeignKey(t => t.StudentId);

        modelBuilder.Entity<GradeEntry>()
            .HasOne(g => g.Enrollment)
            .WithMany(e => e.GradeEntries)
            .HasForeignKey(g => g.EnrollmentId);

        modelBuilder.Entity<GradeEntry>()
            .HasOne(g => g.Transcript)
            .WithMany(t => t.GradeEntries)
            .HasForeignKey(g => g.TranscriptId);

        modelBuilder.Entity<OnlineCourse>()
            .HasOne(o => o.Lms)
            .WithMany(l => l.OnlineCourses)
            .HasForeignKey(o => o.LmsId);

        modelBuilder.Entity<Assignment>()
            .HasOne(a => a.OnlineCourse)
            .WithMany(c => c.Assignments)
            .HasForeignKey(a => a.OnlineCourseId);

        modelBuilder.Entity<Submission>()
            .HasOne(s => s.Assignment)
            .WithMany(a => a.Submissions)
            .HasForeignKey(s => s.AssignmentId);

        modelBuilder.Entity<FinancialAidPackage>()
            .HasOne(a => a.Student)
            .WithMany(s => s.AidPackages)
            .HasForeignKey(a => a.StudentId);

        modelBuilder.Entity<PaymentPlan>()
            .HasOne(p => p.AidPackage)
            .WithMany(a => a.PaymentPlans)
            .HasForeignKey(p => p.AidPackageId);

        modelBuilder.Entity<BillingAccount>()
            .HasOne(b => b.Student)
            .WithMany(s => s.BillingAccounts)
            .HasForeignKey(b => b.StudentId);

        modelBuilder.Entity<TuitionInvoice>()
            .HasOne(i => i.BillingAccount)
            .WithMany(b => b.Invoices)
            .HasForeignKey(i => i.BillingAccountId);

        modelBuilder.Entity<PaymentTransaction>()
            .HasOne(p => p.Invoice)
            .WithMany(i => i.Payments)
            .HasForeignKey(p => p.InvoiceId);

        modelBuilder.Entity<ResearchProject>()
            .HasOne(p => p.ResearchFellow)
            .WithMany(r => r.Projects)
            .HasForeignKey(p => p.ResearchFellowId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ResearchProject>()
            .HasOne(p => p.FacultyGrant)
            .WithMany(g => g.Projects)
            .HasForeignKey(p => p.FacultyGrantId);

        modelBuilder.Entity<FacultyGrant>()
            .HasOne(g => g.Faculty)
            .WithMany(f => f.Grants)
            .HasForeignKey(g => g.FacultyId);

        modelBuilder.Entity<AlumniRelations>()
            .HasMany(r => r.Events)
            .WithOne(e => e.Relations)
            .HasForeignKey(e => e.RelationsId);

        modelBuilder.Entity<AdmissionsOfficerEnrollment>()
            .HasKey(x => new { x.OfficerId, x.EnrollmentId });
        modelBuilder.Entity<AdmissionsOfficerEnrollment>()
            .HasOne(x => x.Officer)
            .WithMany(o => o.EnrollmentAssignments)
            .HasForeignKey(x => x.OfficerId);
        modelBuilder.Entity<AdmissionsOfficerEnrollment>()
            .HasOne(x => x.Enrollment)
            .WithMany(e => e.AdmissionsAssignments)
            .HasForeignKey(x => x.EnrollmentId);

        modelBuilder.Entity<RegistrarTranscript>()
            .HasKey(x => new { x.RegistrarId, x.TranscriptId });
        modelBuilder.Entity<RegistrarTranscript>()
            .HasOne(x => x.Registrar)
            .WithMany(r => r.TranscriptAssignments)
            .HasForeignKey(x => x.RegistrarId);
        modelBuilder.Entity<RegistrarTranscript>()
            .HasOne(x => x.Transcript)
            .WithMany(t => t.RegistrarAssignments)
            .HasForeignKey(x => x.TranscriptId);

        modelBuilder.Entity<StudentCourseOffering>()
            .HasKey(x => new { x.StudentId, x.OfferingId });
        modelBuilder.Entity<StudentCourseOffering>()
            .HasOne(x => x.Student)
            .WithMany(s => s.StudentCourseOfferings)
            .HasForeignKey(x => x.StudentId);
        modelBuilder.Entity<StudentCourseOffering>()
            .HasOne(x => x.Offering)
            .WithMany(o => o.StudentCourseOfferings)
            .HasForeignKey(x => x.OfferingId);

        modelBuilder.Entity<ResearchProjectCourseOffering>()
            .HasKey(x => new { x.ResearchProjectId, x.OfferingId });
        modelBuilder.Entity<ResearchProjectCourseOffering>()
            .HasOne(x => x.ResearchProject)
            .WithMany(p => p.CourseOfferings)
            .HasForeignKey(x => x.ResearchProjectId);
        modelBuilder.Entity<ResearchProjectCourseOffering>()
            .HasOne(x => x.Offering)
            .WithMany(o => o.ResearchProjectLinks)
            .HasForeignKey(x => x.OfferingId);

        modelBuilder.Entity<AlumniEvent>()
            .HasKey(x => new { x.AlumniId, x.EventId });
        modelBuilder.Entity<AlumniEvent>()
            .HasOne(x => x.Alumni)
            .WithMany(a => a.Events)
            .HasForeignKey(x => x.AlumniId);
        modelBuilder.Entity<AlumniEvent>()
            .HasOne(x => x.Event)
            .WithMany(e => e.Alumni)
            .HasForeignKey(x => x.EventId);

        modelBuilder.Entity<AlumniRelationsAlumni>()
            .HasKey(x => new { x.RelationsId, x.AlumniId });
        modelBuilder.Entity<AlumniRelationsAlumni>()
            .HasOne(x => x.Relations)
            .WithMany(r => r.AlumniLinks)
            .HasForeignKey(x => x.RelationsId);
        modelBuilder.Entity<AlumniRelationsAlumni>()
            .HasOne(x => x.Alumni)
            .WithMany(a => a.AlumniRelations)
            .HasForeignKey(x => x.AlumniId);

        modelBuilder.Entity<CourseCurriculum>()
            .Property(c => c.RowVersion)
            .IsRowVersion();
        modelBuilder.Entity<Student>()
            .Property(s => s.RowVersion)
            .IsRowVersion();
        modelBuilder.Entity<Transcript>()
            .Property(t => t.RowVersion)
            .IsRowVersion();
        modelBuilder.Entity<FinancialAidPackage>()
            .Property(f => f.RowVersion)
            .IsRowVersion();
        modelBuilder.Entity<FacultyGrant>()
            .Property(f => f.RowVersion)
            .IsRowVersion();
        modelBuilder.Entity<ResearchFellow>()
            .Property(r => r.RowVersion)
            .IsRowVersion();
    }
}
