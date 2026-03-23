using HospitalApi.Data;
using HospitalApi.Domain;
using HospitalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers;

[ApiController]
[Route("api/patient-records")]
public class PatientRecordsController : ControllerBase
{
    private readonly PatientService _service;

    public PatientRecordsController(PatientService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PatientRecord>> GetAll() => Ok(_service.GetRecords());

    [HttpPost]
    public ActionResult<PatientRecord> Create(PatientRecord record)
    {
        var created = _service.AddRecord(record);
        return CreatedAtAction(nameof(GetAll), new { id = created.Id }, created);
    }
}
