using Api.Domain;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/operations")]
public class OperationsController : ControllerBase
{
    private readonly IGovernmentDataStore _store;

    public OperationsController(IGovernmentDataStore store)
    {
        _store = store;
    }

    [HttpGet("audit")]
    public ActionResult<IEnumerable<AuditTrail>> GetAudit() => Ok(_store.AuditTrails);

    [HttpGet("status")]
    public ActionResult<IEnumerable<ServiceStatus>> GetStatus()
    {
        var statuses = new List<ServiceStatus>
        {
            new() { ServiceName = "IdentityRegistry", Status = "Operational" },
            new() { ServiceName = "TaxCollection", Status = "Operational" },
            new() { ServiceName = "SocialSecurityBenefits", Status = "Operational" }
        };
        return Ok(statuses);
    }
}
