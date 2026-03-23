using LcpUml5.Api.Contracts.Requests;
using LcpUml5.Api.Domain.Entities;
using LcpUml5.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml5.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RiskController : ControllerBase
{
    private readonly BankingDbContext _db;

    public RiskController(BankingDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<RiskAssessment>> Create(CreateRiskAssessmentRequest request)
    {
        var customer = await _db.CustomerProfiles.FindAsync(request.CustomerProfileId);
        if (customer is null) return NotFound("Customer not found");

        var assessment = new RiskAssessment
        {
            CustomerProfileId = request.CustomerProfileId,
            Score = request.Score,
            IdempotencyKey = request.IdempotencyKey
        };

        _db.RiskAssessments.Add(assessment);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = assessment.Id }, assessment);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RiskAssessment>> GetById(int id)
    {
        var assessment = await _db.RiskAssessments.FindAsync(id);
        return assessment is null ? NotFound() : Ok(assessment);
    }
}
