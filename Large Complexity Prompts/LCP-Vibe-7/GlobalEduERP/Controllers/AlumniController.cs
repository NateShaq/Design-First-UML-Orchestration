using GlobalEduERP.Data;
using GlobalEduERP.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlobalEduERP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlumniController : ControllerBase
{
    private readonly InMemoryData _data;

    public AlumniController(InMemoryData data)
    {
        _data = data;
    }

    [HttpGet]
    public ActionResult<IEnumerable<AlumniProfile>> GetAlumni() => _data.Alumni;

    [HttpGet("donations")]
    public ActionResult<IEnumerable<Donation>> GetDonations() => _data.Donations;

    [HttpGet("grants")]
    public ActionResult<IEnumerable<FacultyGrant>> GetGrants() => _data.Grants;
}
