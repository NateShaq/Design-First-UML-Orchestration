using GlobalEduERP.Models;

namespace GlobalEduERP.Data;

public class InMemoryData
{
    public List<AdmissionApplication> Applications { get; } = new();
    public List<Student> Students { get; } = new();
    public List<Course> Courses { get; } = new();
    public List<CourseCurriculum> Curricula { get; } = new();
    public List<AlumniProfile> Alumni { get; } = new();
    public List<Donation> Donations { get; } = new();
    public List<ResearchProject> ResearchProjects { get; } = new();
    public List<FacultyGrant> Grants { get; } = new();

    public InMemoryData()
    {
        Seed();
    }

    private void Seed()
    {
        Curricula.AddRange(new[]
        {
            new CourseCurriculum { Id = 1, ProgramCode = "UG-CS", CreditsRequired = 120, AcademicYear = "2026" },
            new CourseCurriculum { Id = 2, ProgramCode = "PG-DS", CreditsRequired = 36, AcademicYear = "2026" }
        });

        Courses.AddRange(new[]
        {
            new Course { Id = 1, Code = "CS101", Title = "Intro to Computer Science", CourseCurriculumId = 1, FacultyId = 1 },
            new Course { Id = 2, Code = "DS501", Title = "Applied Data Science", CourseCurriculumId = 2, FacultyId = 2 }
        });

        Students.AddRange(new Student[]
        {
            new Undergraduate { Id = 1, StudentNumber = "U0001", FullName = "Ava North", Email = "ava.north@example.edu", AcademicProgramId = 1, IsHonors = true },
            new Postgraduate { Id = 2, StudentNumber = "P0001", FullName = "Liam West", Email = "liam.west@example.edu", AcademicProgramId = 2, ThesisTitle = "AI in Education" },
            new ResearchFellow { Id = 3, StudentNumber = "R0001", FullName = "Noah South", Email = "noah.south@example.edu", AcademicProgramId = 2, ResearchArea = "Learning Analytics", SupervisorFacultyId = 2 }
        });

        Applications.AddRange(new[]
        {
            new AdmissionApplication { Id = 1, ApplicantName = "Sophia Green", ProgramApplied = "UG-CS", SubmittedOn = DateTime.UtcNow.AddDays(-7), Status = "Review" },
            new AdmissionApplication { Id = 2, ApplicantName = "Mia Stone", ProgramApplied = "PG-DS", SubmittedOn = DateTime.UtcNow.AddDays(-2), Status = "Pending" }
        });

        Alumni.AddRange(new[]
        {
            new AlumniProfile { Id = 1, StudentId = 1, CurrentRole = "Software Engineer", Company = "Contoso" },
            new AlumniProfile { Id = 2, StudentId = 2, CurrentRole = "Data Scientist", Company = "Fabrikam" }
        });

        Donations.AddRange(new[]
        {
            new Donation { Id = 1, AlumniProfileId = 1, Amount = 10000, DonatedOn = DateTime.UtcNow.AddMonths(-3), Purpose = "Scholarships" },
            new Donation { Id = 2, AlumniProfileId = 2, Amount = 5000, DonatedOn = DateTime.UtcNow.AddMonths(-1), Purpose = "Research Lab" }
        });

        ResearchProjects.AddRange(new[]
        {
            new ResearchProject { Id = 1, Title = "Adaptive Learning", LeadFacultyId = 2, Budget = 250000 },
            new ResearchProject { Id = 2, Title = "Sustainable Campuses", LeadFacultyId = 1, Budget = 150000 }
        });

        Grants.AddRange(new[]
        {
            new FacultyGrant { Id = 1, FacultyId = 1, GrantTitle = "Cybersecurity Initiative", Amount = 75000, AwardedOn = DateTime.UtcNow.AddMonths(-4) },
            new FacultyGrant { Id = 2, FacultyId = 2, GrantTitle = "AI for Accessibility", Amount = 90000, AwardedOn = DateTime.UtcNow.AddMonths(-2) }
        });
    }
}
