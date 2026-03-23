using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmnichannelRetail.Api.Data;
using OmnichannelRetail.Api.Dto;
using OmnichannelRetail.Api.Models;

namespace OmnichannelRetail.Api.Controllers;

[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly RetailDbContext _context;
    public PaymentsController(RetailDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<PaymentTransaction>> Authorize(PaymentRequestDto dto)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderNumber == dto.OrderNumber);
        if (order == null) return NotFound($"Order {dto.OrderNumber} not found.");

        var payment = new PaymentTransaction
        {
            PaymentTransactionId = Guid.NewGuid(),
            PaymentGatewayId = dto.PaymentGatewayId,
            OrderNumber = dto.OrderNumber,
            Amount = dto.Amount,
            Status = "Authorized",
            TransactionId = Guid.NewGuid()
        };

        order.PaymentTransactionId = payment.PaymentTransactionId;
        order.Status = "AwaitingCapture";

        _context.PaymentTransactions.Add(payment);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return CreatedAtAction(nameof(Get), new { id = payment.PaymentTransactionId }, payment);
    }

    [HttpPost("{id:guid}/capture")]
    public async Task<IActionResult> Capture(Guid id)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var payment = await _context.PaymentTransactions.FirstOrDefaultAsync(p => p.PaymentTransactionId == id);
        if (payment == null) return NotFound();

        payment.Status = "Captured";
        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PaymentTransaction>> Get(Guid id)
    {
        var payment = await _context.PaymentTransactions.FindAsync(id);
        if (payment == null) return NotFound();
        return payment;
    }
}
