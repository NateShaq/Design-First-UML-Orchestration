using HospitalApi.Domain;
using HospitalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers;

[ApiController]
[Route("api/surgery")]
public class SurgicalSchedulingController : ControllerBase
{
    private readonly SurgeryService _service;

    public SurgicalSchedulingController(SurgeryService service)
    {
        _service = service;
    }

    [HttpGet("cases")]
    public ActionResult<IEnumerable<SurgicalCase>> Cases() => Ok(_service.GetCases());

    [HttpPost("cases")]
    public ActionResult<SurgicalCase> Schedule(SurgicalCase surgicalCase)
    {
        var created = _service.ScheduleCase(surgicalCase);
        return CreatedAtAction(nameof(Cases), new { id = created.Id }, created);
    }
}
