using Api.Domain;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/tax")]
public class TaxController : ControllerBase
{
    private readonly IGovernmentDataStore _store;

    public TaxController(IGovernmentDataStore store)
    {
        _store = store;
    }

    [HttpGet("assessments")]
    public ActionResult<IEnumerable<TaxAssessment>> GetAssessments() => Ok(_store.TaxAssessments);

    [HttpPost("assessments")]
    public ActionResult<TaxAssessment> CreateAssessment([FromBody] TaxAssessment assessment)
    {
        var created = _store.CreateTaxAssessment(assessment.TaxPayerId, assessment.Amount, assessment.TaxYear);
        _store.AuditTrails.Add(new AuditTrail { Action = "TaxAssessed", Details = $"Year {assessment.TaxYear}", ActorId = assessment.TaxPayerId });
        return CreatedAtAction(nameof(GetAssessments), new { id = created.Id }, created);
    }

    [HttpPost("payments")]
    public ActionResult<TaxPayment> RecordPayment([FromBody] TaxPayment payment)
    {
        payment.Id = Guid.NewGuid();
        payment.PaidOn = DateTime.UtcNow;
        _store.TaxPayments.Add(payment);
        _store.TaxCollection.Payments.Add(payment);
        _store.TaxCollection.TotalCollected += payment.Amount;
        _store.AuditTrails.Add(new AuditTrail { Action = "TaxPaid", Details = $"Assessment {payment.TaxAssessmentId}", ActorId = null });
        return CreatedAtAction(nameof(GetPayments), new { id = payment.Id }, payment);
    }

    [HttpGet("payments")]
    public ActionResult<IEnumerable<TaxPayment>> GetPayments() => Ok(_store.TaxPayments);

    [HttpGet("collection")]
    public ActionResult<TaxCollection> GetCollection() => Ok(_store.TaxCollection);
}
