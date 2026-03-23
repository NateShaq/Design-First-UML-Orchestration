using System.Data;
using LcpUml8.Api.Data;
using LcpUml8.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml8.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QualityAssuranceController : ControllerBase
{
    private readonly LcpUml8Context _context;

    public QualityAssuranceController(LcpUml8Context context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<QualityAssurance>> Create(QaRequest request, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        var qa = new QualityAssurance();

        if (!string.IsNullOrWhiteSpace(request.ProductId))
        {
            var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);
            if (product == null) return BadRequest("Product not found.");
            qa.Products.Add(product);
        }

        _context.QualityAssurances.Add(qa);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = qa.Id }, qa);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<QualityAssurance>> GetById(string id, CancellationToken cancellationToken)
    {
        var qa = await _context.QualityAssurances
            .Include(q => q.InspectionPlans)
            .AsNoTracking()
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);

        if (qa == null) return NotFound();
        return Ok(qa);
    }

    [HttpPost("{id}/inspection-plans")]
    public async Task<IActionResult> AddInspectionPlan(string id, InspectionPlanRequest request, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        var qa = await _context.QualityAssurances.FirstOrDefaultAsync(q => q.Id == id, cancellationToken);
        if (qa == null) return NotFound();

        var plan = new InspectionPlan
        {
            Method = request.Method,
            QualityAssuranceId = qa.Id
        };

        _context.InspectionPlans.Add(plan);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return NoContent();
    }
}

public record QaRequest(string? ProductId);

public record InspectionPlanRequest(string Method);
