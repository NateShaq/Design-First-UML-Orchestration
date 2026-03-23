using System.Data;
using LcpUml8.Api.Data;
using LcpUml8.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml8.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShopFloorExecutionsController : ControllerBase
{
    private readonly LcpUml8Context _context;

    public ShopFloorExecutionsController(LcpUml8Context context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<ShopFloorExecution>> Create(ShopFloorExecutionRequest request, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        var sfe = new ShopFloorExecution();

        if (request.WorkOrderIds?.Any() == true)
        {
            var workOrders = await _context.WorkOrders
                .Where(wo => request.WorkOrderIds.Contains(wo.Id))
                .ToListAsync(cancellationToken);

            if (workOrders.Count != request.WorkOrderIds.Count)
            {
                return BadRequest("One or more work orders were not found.");
            }

            foreach (var wo in workOrders)
            {
                sfe.WorkOrders.Add(wo);
            }
        }

        _context.ShopFloorExecutions.Add(sfe);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = sfe.Id }, sfe);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ShopFloorExecution>> GetById(string id, CancellationToken cancellationToken)
    {
        var sfe = await _context.ShopFloorExecutions
            .Include(s => s.WorkOrders)
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);

        if (sfe == null) return NotFound();
        return Ok(sfe);
    }
}

public record ShopFloorExecutionRequest(List<string> WorkOrderIds);
