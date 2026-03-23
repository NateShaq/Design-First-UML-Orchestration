using System.Data;
using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Contracts.Responses;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using LcpUml6.Api.Services.Concurrency;
using LcpUml6.Api.Services.Transactions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/usage-billing")]
public class UsageBillingsController : ControllerBase
{
    private readonly GridDbContext _context;
    private readonly TransactionRegistry _registry;
    private readonly ILogger<UsageBillingsController> _logger;

    public UsageBillingsController(GridDbContext context, TransactionRegistry registry, ILogger<UsageBillingsController> logger)
    {
        _context = context;
        _registry = registry;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsageBilling>>> GetAll() =>
        await _context.UsageBillings.AsNoTracking().ToListAsync();

    [HttpGet("{billingId:guid}")]
    public async Task<ActionResult<UsageBilling>> Get(Guid billingId)
    {
        var entity = await _context.UsageBillings.AsNoTracking().FirstOrDefaultAsync(x => x.BillingId == billingId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<UsageBilling>> Create([FromBody] CreateUsageBillingRequest request)
    {
        var entity = new UsageBilling
        {
            BillingId = request.BillingId ?? Guid.NewGuid(),
            MeterId = request.MeterId,
            TariffPlanId = request.TariffPlanId,
            SettlementId = request.SettlementId,
            PeriodStart = request.PeriodStart,
            PeriodEnd = request.PeriodEnd,
            MeasuredKwh = request.MeasuredKwh,
            Notes = request.Notes,
            Revision = 0
        };

        _context.UsageBillings.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { billingId = entity.BillingId }, entity);
    }

    [HttpPost("{billingId:guid}/begin-transaction")]
    public async Task<ActionResult<TransactionTokenResponse>> BeginTransaction(Guid billingId)
    {
        var billing = await _context.UsageBillings.AsNoTracking().FirstOrDefaultAsync(x => x.BillingId == billingId);
        if (billing is null) return NotFound();

        var txId = _registry.Register(billingId, billing.RowVersion);
        var token = ConcurrencyToken.Encode(billing.RowVersion);
        return new TransactionTokenResponse(txId, token, billing.Revision);
    }

    [HttpPost("{billingId:guid}/commit-transaction")]
    public async Task<IActionResult> Commit(Guid billingId, [FromBody] CommitTransactionRequest request)
    {
        if (!_registry.TryRemove(request.TxId, out var entry) || entry!.EntityId != billingId)
        {
            return BadRequest(new { message = "Unknown or expired transaction id." });
        }

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var entity = await _context.UsageBillings.FirstOrDefaultAsync(x => x.BillingId == billingId);
        if (entity is null)
        {
            await tx.RollbackAsync();
            return NotFound();
        }

        _context.Entry(entity).Property(e => e.RowVersion).OriginalValue = ConcurrencyToken.Decode(request.Token);
        entity.Revision += 1;
        entity.Notes = request.Notes ?? entity.Notes;

        try
        {
            await _context.SaveChangesAsync();
            await tx.CommitAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            await tx.RollbackAsync();
            _logger.LogWarning(ex, "UsageBilling concurrency conflict for {BillingId}", billingId);
            return Conflict(new { message = "Stale token. Reload and retry." });
        }
    }

    [HttpPost("{billingId:guid}/rollback-transaction")]
    public IActionResult Rollback(Guid billingId, [FromBody] CommitTransactionRequest request)
    {
        _registry.TryRemove(request.TxId, out _);
        return Ok(new { message = "Transaction rolled back", billingId });
    }
}
