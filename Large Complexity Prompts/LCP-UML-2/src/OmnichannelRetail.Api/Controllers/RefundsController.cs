using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmnichannelRetail.Api.Data;
using OmnichannelRetail.Api.Dto;
using OmnichannelRetail.Api.Models;

namespace OmnichannelRetail.Api.Controllers;

[ApiController]
[Route("api/refunds")]
public class RefundsController : ControllerBase
{
    private readonly RetailDbContext _context;
    public RefundsController(RetailDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<RefundProcessor>> IssueRefund(RefundRequestDto dto)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var rma = await _context.ReturnAuthorizations.FirstOrDefaultAsync(r => r.RmaNumber == dto.RmaNumber);
        if (rma == null) return NotFound($"RMA {dto.RmaNumber} not found.");

        var refund = new RefundProcessor
        {
            RefundId = Guid.NewGuid(),
            PaymentGatewayId = dto.PaymentGatewayId,
            Amount = dto.Amount,
            Method = dto.Method,
            TransactionId = Guid.NewGuid()
        };

        rma.RefundId = refund.RefundId;
        rma.Status = "Refunded";

        _context.RefundProcessors.Add(refund);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return CreatedAtAction(nameof(Get), new { id = refund.RefundId }, refund);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<RefundProcessor>> Get(Guid id)
    {
        var refund = await _context.RefundProcessors.FindAsync(id);
        if (refund == null) return NotFound();
        return refund;
    }
}
