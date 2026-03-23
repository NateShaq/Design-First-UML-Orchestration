using System.Data;
using LcpUml8.Api.Data;
using LcpUml8.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml8.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EngineeringDesignsController : ControllerBase
{
    private readonly LcpUml8Context _context;

    public EngineeringDesignsController(LcpUml8Context context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<EngineeringDesign>> Create(DesignRequest request, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        var design = new EngineeringDesign
        {
            LifecycleState = request.LifecycleState
        };

        _context.EngineeringDesigns.Add(design);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = design.Id }, design);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EngineeringDesign>> GetById(string id, CancellationToken cancellationToken)
    {
        var design = await _context.EngineeringDesigns
            .Include(d => d.CadModels)
            .Include(d => d.Drawings)
            .Include(d => d.Revisions)
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        if (design == null) return NotFound();
        return Ok(design);
    }

    [HttpPost("{id}/commit")]
    public async Task<IActionResult> CommitLifecycle(string id, CommitRequest request, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        var design = await _context.EngineeringDesigns.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if (design == null) return NotFound();

        design.LifecycleState = request.TargetState;

        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}

public record DesignRequest(string LifecycleState);

public record CommitRequest(string TargetState);
