-- Global Higher Education ERP schema
CREATE TABLE Department (
    Id INT PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    Dean NVARCHAR(150) NULL
);

CREATE TABLE AcademicProgram (
    Id INT PRIMARY KEY,
    Code NVARCHAR(50) NOT NULL,
    Name NVARCHAR(200) NOT NULL,
    DepartmentId INT NOT NULL REFERENCES Department(Id)
);

CREATE TABLE CourseCurriculum (
    Id INT PRIMARY KEY,
    ProgramCode NVARCHAR(50) NOT NULL,
    CreditsRequired INT NOT NULL,
    AcademicYear NVARCHAR(10) NOT NULL
);

CREATE TABLE Course (
    Id INT PRIMARY KEY,
    Code NVARCHAR(20) NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    CourseCurriculumId INT NOT NULL REFERENCES CourseCurriculum(Id),
    FacultyId INT NOT NULL
);

CREATE TABLE Module (
    Id INT PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    CourseId INT NOT NULL REFERENCES Course(Id),
    [Order] INT NOT NULL
);

CREATE TABLE Assignment (
    Id INT PRIMARY KEY,
    ModuleId INT NOT NULL REFERENCES Module(Id),
    Title NVARCHAR(150) NOT NULL,
    DueDate DATETIME2 NOT NULL
);

CREATE TABLE Grade (
    Id INT PRIMARY KEY,
    AssignmentId INT NOT NULL REFERENCES Assignment(Id),
    StudentId INT NOT NULL,
    LetterGrade NVARCHAR(5) NOT NULL,
    Points DECIMAL(5,2) NOT NULL
);

CREATE TABLE Semester (
    Id INT PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL
);

CREATE TABLE Student (
    Id INT PRIMARY KEY,
    StudentNumber NVARCHAR(50) NOT NULL,
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    AcademicProgramId INT NOT NULL REFERENCES AcademicProgram(Id)
);

CREATE TABLE Undergraduate (
    Id INT PRIMARY KEY REFERENCES Student(Id),
    IsHonors BIT NOT NULL DEFAULT 0
);

CREATE TABLE Postgraduate (
    Id INT PRIMARY KEY REFERENCES Student(Id),
    ThesisTitle NVARCHAR(250) NULL
);

CREATE TABLE ResearchFellow (
    Id INT PRIMARY KEY REFERENCES Student(Id),
    ResearchArea NVARCHAR(200) NULL,
    SupervisorFacultyId INT NOT NULL
);

CREATE TABLE AdmissionApplication (
    Id INT PRIMARY KEY,
    ApplicantName NVARCHAR(200) NOT NULL,
    ProgramApplied NVARCHAR(50) NOT NULL,
    SubmittedOn DATETIME2 NOT NULL,
    Status NVARCHAR(50) NOT NULL
);

CREATE TABLE Enrollment (
    Id INT PRIMARY KEY,
    StudentId INT NOT NULL REFERENCES Student(Id),
    CourseId INT NOT NULL REFERENCES Course(Id),
    EnrolledOn DATETIME2 NOT NULL
);

CREATE TABLE Faculty (
    Id INT PRIMARY KEY,
    FullName NVARCHAR(200) NOT NULL,
    Email NVARCHAR(200) NOT NULL,
    DepartmentId INT NOT NULL REFERENCES Department(Id)
);

CREATE TABLE FacultyGrant (
    Id INT PRIMARY KEY,
    FacultyId INT NOT NULL REFERENCES Faculty(Id),
    GrantTitle NVARCHAR(250) NOT NULL,
    Amount DECIMAL(12,2) NOT NULL,
    AwardedOn DATETIME2 NOT NULL
);

CREATE TABLE Campus (
    Id INT PRIMARY KEY,
    Name NVARCHAR(150) NOT NULL,
    City NVARCHAR(150) NOT NULL
);

CREATE TABLE Classroom (
    Id INT PRIMARY KEY,
    RoomNumber NVARCHAR(50) NOT NULL,
    CampusId INT NOT NULL REFERENCES Campus(Id),
    Capacity INT NOT NULL
);

CREATE TABLE FinancialAidPackage (
    Id INT PRIMARY KEY,
    StudentId INT NOT NULL REFERENCES Student(Id),
    ScholarshipAmount DECIMAL(12,2) NOT NULL,
    LoanAmount DECIMAL(12,2) NOT NULL,
    WorkStudyHours DECIMAL(5,2) NOT NULL
);

CREATE TABLE TuitionInvoice (
    Id INT PRIMARY KEY,
    StudentId INT NOT NULL REFERENCES Student(Id),
    Amount DECIMAL(12,2) NOT NULL,
    DueDate DATE NOT NULL,
    IsPaid BIT NOT NULL DEFAULT 0
);

CREATE TABLE Payment (
    Id INT PRIMARY KEY,
    TuitionInvoiceId INT NOT NULL REFERENCES TuitionInvoice(Id),
    Amount DECIMAL(12,2) NOT NULL,
    PaidOn DATETIME2 NOT NULL,
    Method NVARCHAR(50) NOT NULL
);

CREATE TABLE Transcript (
    Id INT PRIMARY KEY,
    StudentId INT NOT NULL REFERENCES Student(Id),
    GPA DECIMAL(3,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL
);

CREATE TABLE LibraryItem (
    Id INT PRIMARY KEY,
    Title NVARCHAR(250) NOT NULL,
    ItemType NVARCHAR(50) NOT NULL,
    ISBN NVARCHAR(50) NULL
);

CREATE TABLE Event (
    Id INT PRIMARY KEY,
    Title NVARCHAR(200) NOT NULL,
    ScheduledOn DATETIME2 NOT NULL,
    Location NVARCHAR(150) NOT NULL
);

CREATE TABLE AlumniProfile (
    Id INT PRIMARY KEY,
    StudentId INT NOT NULL REFERENCES Student(Id),
    CurrentRole NVARCHAR(150) NOT NULL,
    Company NVARCHAR(150) NOT NULL
);

CREATE TABLE Donation (
    Id INT PRIMARY KEY,
    AlumniProfileId INT NOT NULL REFERENCES AlumniProfile(Id),
    Amount DECIMAL(12,2) NOT NULL,
    DonatedOn DATETIME2 NOT NULL,
    Purpose NVARCHAR(200) NOT NULL
);

CREATE TABLE InternshipPlacement (
    Id INT PRIMARY KEY,
    StudentId INT NOT NULL REFERENCES Student(Id),
    EmployerPartnerId INT NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL
);

CREATE TABLE EmployerPartner (
    Id INT PRIMARY KEY,
    Name NVARCHAR(200) NOT NULL,
    Industry NVARCHAR(100) NOT NULL
);

CREATE TABLE ResearchProject (
    Id INT PRIMARY KEY,
    Title NVARCHAR(250) NOT NULL,
    LeadFacultyId INT NOT NULL REFERENCES Faculty(Id),
    Budget DECIMAL(12,2) NOT NULL
);

CREATE TABLE LearningResource (
    Id INT PRIMARY KEY,
    Title NVARCHAR(250) NOT NULL,
    ResourceType NVARCHAR(100) NOT NULL,
    Url NVARCHAR(300) NOT NULL
);
