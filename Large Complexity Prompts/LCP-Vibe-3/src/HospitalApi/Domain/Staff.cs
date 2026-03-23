namespace HospitalApi.Domain;

public class Physician
{
    public Guid Id { get; set; }
    public string Npi { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
}

public class Nurse
{
    public Guid Id { get; set; }
    public string LicenseNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Unit { get; set; } = string.Empty;
}

public class Department
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CostCenter { get; set; } = string.Empty;
}

public class Room
{
    public Guid Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string Ward { get; set; } = string.Empty;
    public bool IsIsolation { get; set; }
}

public class SurgicalTeamMember
{
    public Guid Id { get; set; }
    public Guid SurgicalCaseId { get; set; }
    public string Role { get; set; } = string.Empty;
    public Guid StaffId { get; set; }
}
