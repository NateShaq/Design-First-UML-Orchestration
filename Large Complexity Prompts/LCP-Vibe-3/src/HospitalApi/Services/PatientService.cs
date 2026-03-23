using HospitalApi.Data;
using HospitalApi.Domain;

namespace HospitalApi.Services;

public class PatientService
{
    private readonly HospitalRepository _repo;

    public PatientService(HospitalRepository repo)
    {
        _repo = repo;
    }

    public IEnumerable<PatientRecord> GetRecords() => _repo.PatientRecords;

    public PatientRecord AddRecord(PatientRecord record)
    {
        record.Id = record.Id == Guid.Empty ? Guid.NewGuid() : record.Id;
        _repo.PatientRecords.Add(record);
        return record;
    }
}
