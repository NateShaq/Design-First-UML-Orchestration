using GlobalEduERP.Data;
using GlobalEduERP.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlobalEduERP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AdmissionsController : ControllerBase
{
    private readonly InMemoryData _data;

    public AdmissionsController(InMemoryData data)
    {
        _data = data;
    }

    [HttpGet("applications")]
    public ActionResult<IEnumerable<AdmissionApplication>> GetApplications() => _data.Applications;

    [HttpPost("applications")]
    public ActionResult<AdmissionApplication> SubmitApplication(AdmissionApplication application)
    {
        application.Id = _data.Applications.Any() ? _data.Applications.Max(a => a.Id) + 1 : 1;
        application.SubmittedOn = DateTime.UtcNow;
        _data.Applications.Add(application);
        return CreatedAtAction(nameof(GetApplications), new { id = application.Id }, application);
    }

    [HttpGet("students")]
    public ActionResult<IEnumerable<Student>> GetStudents() => _data.Students;
}
