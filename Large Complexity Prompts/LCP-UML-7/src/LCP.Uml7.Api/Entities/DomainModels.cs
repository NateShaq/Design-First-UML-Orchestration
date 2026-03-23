using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCP.Uml7.Api.Entities;

public abstract class Student
{
    [Key]
    public Guid StudentId { get; set; }
    [MaxLength(200)]
    public string LegalName { get; set; } = string.Empty;
    [MaxLength(320)]
    public string Email { get; set; } = string.Empty;
    public int Revision { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public ICollection<Transcript> Transcripts { get; set; } = new List<Transcript>();
    public ICollection<FinancialAidPackage> AidPackages { get; set; } = new List<FinancialAidPackage>();
    public ICollection<BillingAccount> BillingAccounts { get; set; } = new List<BillingAccount>();
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<StudentCourseOffering> StudentCourseOfferings { get; set; } = new List<StudentCourseOffering>();
}

public class Undergraduate : Student { }

public class Postgraduate : Student { }

public abstract class AcademicStaff
{
    [Key]
    public Guid StaffId { get; set; }
    public string FacultyRank { get; set; } = string.Empty;
}

public class Faculty : AcademicStaff
{
    public string DepartmentCode { get; set; } = string.Empty;
    public ICollection<CourseOffering> CourseOfferings { get; set; } = new List<CourseOffering>();
    public ICollection<FacultyGrant> Grants { get; set; } = new List<FacultyGrant>();
}

public class ResearchFellow : AcademicStaff
{
    public Guid FellowshipId { get; set; }
    public int Revision { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public ICollection<ResearchProject> Projects { get; set; } = new List<ResearchProject>();
}

public class AdmissionsOfficer
{
    [Key]
    public Guid OfficerId { get; set; }
    public ICollection<AdmissionsOfficerEnrollment> EnrollmentAssignments { get; set; } = new List<AdmissionsOfficerEnrollment>();
}

public class Registrar
{
    [Key]
    public Guid RegistrarId { get; set; }
    public ICollection<RegistrarTranscript> TranscriptAssignments { get; set; } = new List<RegistrarTranscript>();
}

public class Alumni
{
    [Key]
    public Guid AlumniId { get; set; }
    public int GraduationYear { get; set; }
    public ICollection<AlumniEvent> Events { get; set; } = new List<AlumniEvent>();
    public ICollection<AlumniRelationsAlumni> AlumniRelations { get; set; } = new List<AlumniRelationsAlumni>();
}

public class CourseCurriculum
{
    [Key]
    public Guid CurriculumId { get; set; }
    public string ProgramCode { get; set; } = string.Empty;
    public int Revision { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public ICollection<CourseModule> Modules { get; set; } = new List<CourseModule>();
}

public class CourseModule
{
    [Key]
    public Guid ModuleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public int CreditHours { get; set; }
    public Guid CurriculumId { get; set; }
    public CourseCurriculum Curriculum { get; set; } = null!;
    public ICollection<CourseOffering> Offerings { get; set; } = new List<CourseOffering>();
}

public class CourseOffering
{
    [Key]
    public Guid OfferingId { get; set; }
    public string Term { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public Guid ModuleId { get; set; }
    public CourseModule Module { get; set; } = null!;
    public Guid? FacultyId { get; set; }
    public Faculty? Faculty { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
    public ICollection<StudentCourseOffering> StudentCourseOfferings { get; set; } = new List<StudentCourseOffering>();
    public ICollection<ResearchProjectCourseOffering> ResearchProjectLinks { get; set; } = new List<ResearchProjectCourseOffering>();
}

public class Enrollment
{
    [Key]
    public Guid EnrollmentId { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public Guid OfferingId { get; set; }
    public CourseOffering Offering { get; set; } = null!;
    public string Status { get; set; } = "Pending";
    public ICollection<GradeEntry> GradeEntries { get; set; } = new List<GradeEntry>();
    public ICollection<AdmissionsOfficerEnrollment> AdmissionsAssignments { get; set; } = new List<AdmissionsOfficerEnrollment>();
}

public class Transcript
{
    [Key]
    public Guid TranscriptId { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int Revision { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public ICollection<GradeEntry> GradeEntries { get; set; } = new List<GradeEntry>();
    public ICollection<RegistrarTranscript> RegistrarAssignments { get; set; } = new List<RegistrarTranscript>();
}

public class GradeEntry
{
    [Key]
    public Guid GradeEntryId { get; set; }
    public Guid EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;
    public Guid TranscriptId { get; set; }
    public Transcript Transcript { get; set; } = null!;
    [MaxLength(2)]
    public string LetterGrade { get; set; } = string.Empty;
}

public class LearningManagementSystem
{
    [Key]
    public Guid LmsId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<OnlineCourse> OnlineCourses { get; set; } = new List<OnlineCourse>();
}

public class OnlineCourse
{
    [Key]
    public Guid OnlineCourseId { get; set; }
    public string PlatformUrl { get; set; } = string.Empty;
    public Guid LmsId { get; set; }
    public LearningManagementSystem Lms { get; set; } = null!;
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
}

public class Assignment
{
    [Key]
    public Guid AssignmentId { get; set; }
    public DateOnly DueDate { get; set; }
    public Guid OnlineCourseId { get; set; }
    public OnlineCourse OnlineCourse { get; set; } = null!;
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}

public class Submission
{
    [Key]
    public Guid SubmissionId { get; set; }
    public DateTime Timestamp { get; set; }
    public Guid AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;
}

public abstract class FinancialAidPackage
{
    [Key]
    public Guid AidPackageId { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public int Revision { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public ICollection<PaymentPlan> PaymentPlans { get; set; } = new List<PaymentPlan>();
}

public class Scholarship : FinancialAidPackage
{
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
}

public class LoanAid : FinancialAidPackage
{
    [Column(TypeName = "decimal(18,2)")]
    public decimal Principal { get; set; }
}

public class PaymentPlan
{
    [Key]
    public Guid PlanId { get; set; }
    public int Installments { get; set; }
    public Guid AidPackageId { get; set; }
    public FinancialAidPackage AidPackage { get; set; } = null!;
}

public class BillingAccount
{
    [Key]
    public Guid BillingAccountId { get; set; }
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Balance { get; set; }
    public ICollection<TuitionInvoice> Invoices { get; set; } = new List<TuitionInvoice>();
}

public class TuitionInvoice
{
    [Key]
    public Guid InvoiceId { get; set; }
    public Guid BillingAccountId { get; set; }
    public BillingAccount BillingAccount { get; set; } = null!;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    public ICollection<PaymentTransaction> Payments { get; set; } = new List<PaymentTransaction>();
}

public class PaymentTransaction
{
    [Key]
    public Guid PaymentId { get; set; }
    public Guid InvoiceId { get; set; }
    public TuitionInvoice Invoice { get; set; } = null!;
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
}

public class ResearchProject
{
    [Key]
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public Guid? ResearchFellowId { get; set; }
    public ResearchFellow? ResearchFellow { get; set; }
    public Guid? FacultyGrantId { get; set; }
    public FacultyGrant? FacultyGrant { get; set; }
    public ICollection<ResearchProjectCourseOffering> CourseOfferings { get; set; } = new List<ResearchProjectCourseOffering>();
}

public class FacultyGrant
{
    [Key]
    public Guid GrantId { get; set; }
    public Guid FacultyId { get; set; }
    public Faculty Faculty { get; set; } = null!;
    public int Revision { get; set; }
    [Timestamp]
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    public ICollection<ResearchProject> Projects { get; set; } = new List<ResearchProject>();
}

public class AlumniRelations
{
    [Key]
    public Guid RelationsId { get; set; }
    public string Strategy { get; set; } = string.Empty;
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<AlumniRelationsAlumni> AlumniLinks { get; set; } = new List<AlumniRelationsAlumni>();
}

public class Event
{
    [Key]
    public Guid EventId { get; set; }
    public DateOnly Date { get; set; }
    public Guid RelationsId { get; set; }
    public AlumniRelations Relations { get; set; } = null!;
    public ICollection<AlumniEvent> Alumni { get; set; } = new List<AlumniEvent>();
}

public class AdmissionsOfficerEnrollment
{
    public Guid OfficerId { get; set; }
    public AdmissionsOfficer Officer { get; set; } = null!;
    public Guid EnrollmentId { get; set; }
    public Enrollment Enrollment { get; set; } = null!;
}

public class RegistrarTranscript
{
    public Guid RegistrarId { get; set; }
    public Registrar Registrar { get; set; } = null!;
    public Guid TranscriptId { get; set; }
    public Transcript Transcript { get; set; } = null!;
}

public class StudentCourseOffering
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public Guid OfferingId { get; set; }
    public CourseOffering Offering { get; set; } = null!;
}

public class ResearchProjectCourseOffering
{
    public Guid ResearchProjectId { get; set; }
    public ResearchProject ResearchProject { get; set; } = null!;
    public Guid OfferingId { get; set; }
    public CourseOffering Offering { get; set; } = null!;
}

public class AlumniEvent
{
    public Guid AlumniId { get; set; }
    public Alumni Alumni { get; set; } = null!;
    public Guid EventId { get; set; }
    public Event Event { get; set; } = null!;
}

public class AlumniRelationsAlumni
{
    public Guid RelationsId { get; set; }
    public AlumniRelations Relations { get; set; } = null!;
    public Guid AlumniId { get; set; }
    public Alumni Alumni { get; set; } = null!;
}
