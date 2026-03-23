using Api.Domain;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/benefits")]
public class BenefitsController : ControllerBase
{
    private readonly IGovernmentDataStore _store;

    public BenefitsController(IGovernmentDataStore store)
    {
        _store = store;
    }

    [HttpGet]
    public ActionResult<IEnumerable<SocialSecurityBenefits>> GetBenefits() => Ok(_store.Benefits);

    [HttpPost]
    public ActionResult<SocialSecurityBenefits> CreateBenefit([FromBody] SocialSecurityBenefits benefit)
    {
        var created = _store.CreateBenefit(benefit.CitizenId, benefit.MonthlyAmount);
        _store.AuditTrails.Add(new AuditTrail { ActorId = benefit.CitizenId, Action = "BenefitCreated", Details = benefit.MonthlyAmount.ToString("F2") });
        return CreatedAtAction(nameof(GetBenefits), new { id = created.Id }, created);
    }

    [HttpPost("claims")]
    public ActionResult<BenefitClaim> CreateClaim([FromBody] BenefitClaim claim)
    {
        claim.Id = Guid.NewGuid();
        claim.Status = "Pending";
        _store.BenefitClaims.Add(claim);
        _store.AuditTrails.Add(new AuditTrail { ActorId = claim.CitizenId, Action = "BenefitClaimed", Details = claim.BenefitType });
        return CreatedAtAction(nameof(GetClaims), new { id = claim.Id }, claim);
    }

    [HttpGet("claims")]
    public ActionResult<IEnumerable<BenefitClaim>> GetClaims() => Ok(_store.BenefitClaims);
}
