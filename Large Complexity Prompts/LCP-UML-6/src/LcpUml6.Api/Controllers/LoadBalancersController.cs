using System.Data;
using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Contracts.Responses;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using LcpUml6.Api.Services.Concurrency;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/load-balancers")]
public class LoadBalancersController : ControllerBase
{
    private readonly GridDbContext _context;
    private readonly ILogger<LoadBalancersController> _logger;

    public LoadBalancersController(GridDbContext context, ILogger<LoadBalancersController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoadBalancer>>> GetAll() =>
        await _context.LoadBalancers.AsNoTracking().ToListAsync();

    [HttpGet("{balancerId:guid}")]
    public async Task<ActionResult<LoadBalancer>> Get(Guid balancerId)
    {
        var entity = await _context.LoadBalancers.AsNoTracking().FirstOrDefaultAsync(x => x.BalancerId == balancerId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<LoadBalancer>> Create([FromBody] CreateLoadBalancerRequest request)
    {
        var entity = new LoadBalancer
        {
            BalancerId = request.BalancerId ?? Guid.NewGuid(),
            GridConnectionId = request.GridConnectionId,
            DispatchPlan = request.DispatchPlan,
            Revision = 0
        };

        _context.LoadBalancers.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { balancerId = entity.BalancerId }, entity);
    }

    [HttpPost("{balancerId:guid}/begin-update")]
    public async Task<ActionResult<BeginUpdateResponse>> BeginUpdate(Guid balancerId, [FromQuery] string? txId = null)
    {
        var entity = await _context.LoadBalancers.AsNoTracking().FirstOrDefaultAsync(x => x.BalancerId == balancerId);
        if (entity is null) return NotFound();
        return new BeginUpdateResponse(ConcurrencyToken.Encode(entity.RowVersion), entity.Revision);
    }

    [HttpPost("{balancerId:guid}/commit-update")]
    public async Task<IActionResult> CommitUpdate(Guid balancerId, [FromBody] CommitUpdateRequest request)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var entity = await _context.LoadBalancers.FirstOrDefaultAsync(x => x.BalancerId == balancerId);
        if (entity is null)
        {
            await tx.RollbackAsync();
            return NotFound();
        }

        _context.Entry(entity).Property(e => e.RowVersion).OriginalValue = ConcurrencyToken.Decode(request.Token);
        entity.Revision += 1;
        entity.DispatchPlan = request.Notes ?? entity.DispatchPlan;

        try
        {
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await tx.RollbackAsync();
            _logger.LogWarning(ex, "LoadBalancer concurrency conflict for {BalancerId}", balancerId);
            return Conflict(new { message = "Stale token. Reload and retry." });
        }
    }

    [HttpPost("{balancerId:guid}/rollback-update")]
    public async Task<IActionResult> RollbackUpdate(Guid balancerId, [FromBody] CommitUpdateRequest request)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var entity = await _context.LoadBalancers.FirstOrDefaultAsync(x => x.BalancerId == balancerId);
        if (entity is null)
        {
            await tx.RollbackAsync();
            return NotFound();
        }

        _context.Entry(entity).Property(e => e.RowVersion).OriginalValue = ConcurrencyToken.Decode(request.Token);
        await tx.RollbackAsync();
        return Ok(new { message = "Rolled back", revision = entity.Revision });
    }
}
