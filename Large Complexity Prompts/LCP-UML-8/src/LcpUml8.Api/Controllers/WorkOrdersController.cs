using System.Data;
using LcpUml8.Api.Data;
using LcpUml8.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml8.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkOrdersController : ControllerBase
{
    private readonly LcpUml8Context _context;

    public WorkOrdersController(LcpUml8Context context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<WorkOrder>> Create(CreateWorkOrderRequest request, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);
        if (product == null) return BadRequest("Product not found.");

        var workOrder = new WorkOrder
        {
            ProductId = request.ProductId,
            ScheduleDate = request.ScheduleDate
        };

        _context.WorkOrders.Add(workOrder);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = workOrder.Id }, workOrder);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkOrder>> GetById(string id, CancellationToken cancellationToken)
    {
        var workOrder = await _context.WorkOrders
            .Include(wo => wo.Operations)
            .AsNoTracking()
            .FirstOrDefaultAsync(wo => wo.Id == id, cancellationToken);

        if (workOrder == null) return NotFound();
        return Ok(workOrder);
    }

    [HttpPost("{id}/operations")]
    public async Task<IActionResult> AddOperation(string id, OperationRequest request, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);

        var workOrder = await _context.WorkOrders.FirstOrDefaultAsync(wo => wo.Id == id, cancellationToken);
        if (workOrder == null) return NotFound();

        var operation = new Operation
        {
            WorkOrderId = workOrder.Id,
            Seq = request.Seq,
            WorkCenterId = request.WorkCenterId,
            ManufacturingProcessId = request.ManufacturingProcessId
        };

        _context.Operations.Add(operation);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}

public record CreateWorkOrderRequest(DateTime ScheduleDate, string ProductId);

public record OperationRequest(int Seq, string? WorkCenterId, string? ManufacturingProcessId);
