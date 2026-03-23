using HospitalApi.Data;
using HospitalApi.Domain;

namespace HospitalApi.Services;

public class SurgeryService
{
    private readonly HospitalRepository _repo;

    public SurgeryService(HospitalRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<SurgicalCase> GetCases() => _repo.SurgicalCases;

    public SurgicalCase ScheduleCase(SurgicalCase surgicalCase)
    {
        surgicalCase.Id = surgicalCase.Id == Guid.Empty ? Guid.NewGuid() : surgicalCase.Id;
        _repo.SurgicalCases.Add(surgicalCase);
        return surgicalCase;
    }
}
