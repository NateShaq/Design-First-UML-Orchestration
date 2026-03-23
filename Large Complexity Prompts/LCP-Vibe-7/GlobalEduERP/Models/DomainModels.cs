namespace GlobalEduERP.Models;

public class Student
{
    public int Id { get; set; }
    public string StudentNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int AcademicProgramId { get; set; }
}

public class Undergraduate : Student
{
    public bool IsHonors { get; set; }
}

public class Postgraduate : Student
{
    public string ThesisTitle { get; set; } = string.Empty;
}

public class ResearchFellow : Student
{
    public string ResearchArea { get; set; } = string.Empty;
    public int SupervisorFacultyId { get; set; }
}

public class AdmissionApplication
{
    public int Id { get; set; }
    public string ApplicantName { get; set; } = string.Empty;
    public string ProgramApplied { get; set; } = string.Empty;
    public DateTime SubmittedOn { get; set; }
    public string Status { get; set; } = "Pending";
}

public class Enrollment
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int CourseId { get; set; }
    public DateTime EnrolledOn { get; set; }
}

public class Course
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public int CourseCurriculumId { get; set; }
    public int FacultyId { get; set; }
}

public class CourseCurriculum
{
    public int Id { get; set; }
    public string ProgramCode { get; set; } = string.Empty;
    public int CreditsRequired { get; set; }
    public string AcademicYear { get; set; } = string.Empty;
}

public class Module
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int CourseId { get; set; }
    public int Order { get; set; }
}

public class Assignment
{
    public int Id { get; set; }
    public int ModuleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
}

public class Grade
{
    public int Id { get; set; }
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    public string LetterGrade { get; set; } = "IP";
    public decimal Points { get; set; }
}

public class Semester
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class AcademicProgram
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}

public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Dean { get; set; } = string.Empty;
}

public class Faculty
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
}

public class FacultyGrant
{
    public int Id { get; set; }
    public int FacultyId { get; set; }
    public string GrantTitle { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime AwardedOn { get; set; }
}

public class Campus
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
}

public class Classroom
{
    public int Id { get; set; }
    public string RoomNumber { get; set; } = string.Empty;
    public int CampusId { get; set; }
    public int Capacity { get; set; }
}

public class FinancialAidPackage
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public decimal ScholarshipAmount { get; set; }
    public decimal LoanAmount { get; set; }
    public decimal WorkStudyHours { get; set; }
}

public class TuitionInvoice
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPaid { get; set; }
}

public class Payment
{
    public int Id { get; set; }
    public int TuitionInvoiceId { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaidOn { get; set; }
    public string Method { get; set; } = string.Empty;
}

public class Transcript
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public decimal GPA { get; set; }
    public string Status { get; set; } = "In Progress";
}

public class LibraryItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ItemType { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
}

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime ScheduledOn { get; set; }
    public string Location { get; set; } = string.Empty;
}

public class AlumniProfile
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public string CurrentRole { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
}

public class Donation
{
    public int Id { get; set; }
    public int AlumniProfileId { get; set; }
    public decimal Amount { get; set; }
    public DateTime DonatedOn { get; set; }
    public string Purpose { get; set; } = string.Empty;
}

public class InternshipPlacement
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int EmployerPartnerId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}

public class EmployerPartner
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Industry { get; set; } = string.Empty;
}

public class ResearchProject
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public int LeadFacultyId { get; set; }
    public decimal Budget { get; set; }
}

public class LearningResource
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ResourceType { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}
