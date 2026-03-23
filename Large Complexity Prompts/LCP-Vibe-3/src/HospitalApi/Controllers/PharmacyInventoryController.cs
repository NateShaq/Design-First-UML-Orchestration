using HospitalApi.Domain;
using HospitalApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace HospitalApi.Controllers;

[ApiController]
[Route("api/pharmacy")]
public class PharmacyInventoryController : ControllerBase
{
    private readonly PharmacyService _service;

    public PharmacyInventoryController(PharmacyService service)
    {
        _service = service;
    }

    [HttpGet]
    public ActionResult<IEnumerable<PharmacyItem>> GetInventory() => Ok(_service.GetInventory());

    [HttpPost("dispense")]
    public ActionResult<DispenseEvent> Dispense(DispenseEvent request)
    {
        var result = _service.Dispense(request);
        return Ok(result);
    }
}
