using System.Data;
using LCP.Uml7.Api.Data;
using LCP.Uml7.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LCP.Uml7.Api.Controllers;

[ApiController]
[Route("api/billing")]
public class BillingController : ControllerBase
{
    private readonly UniversityDbContext _context;

    public BillingController(UniversityDbContext context)
    {
        _context = context;
    }

    [HttpPost("accounts")]
    public async Task<ActionResult<BillingAccount>> CreateAccount([FromBody] BillingAccountCreateDto dto)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var studentExists = await _context.Students.AnyAsync(s => s.StudentId == dto.StudentId);
        if (!studentExists) return BadRequest("Student not found");

        var account = new BillingAccount
        {
            BillingAccountId = Guid.NewGuid(),
            StudentId = dto.StudentId,
            Balance = 0m
        };

        _context.BillingAccounts.Add(account);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return CreatedAtAction(nameof(GetAccount), new { id = account.BillingAccountId }, account);
    }

    [HttpGet("accounts/{id:guid}")]
    public async Task<ActionResult<BillingAccount>> GetAccount(Guid id)
    {
        var account = await _context.BillingAccounts
            .Include(a => a.Invoices)
            .FirstOrDefaultAsync(a => a.BillingAccountId == id);
        return account is null ? NotFound() : Ok(account);
    }

    [HttpPost("accounts/{accountId:guid}/invoices")]
    public async Task<ActionResult<TuitionInvoice>> CreateInvoice(Guid accountId, [FromBody] InvoiceCreateDto dto)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var account = await _context.BillingAccounts.FindAsync(accountId);
        if (account is null) return NotFound("Billing account not found");

        var invoice = new TuitionInvoice
        {
            InvoiceId = Guid.NewGuid(),
            BillingAccountId = accountId,
            Amount = dto.Amount
        };

        account.Balance += dto.Amount;
        _context.TuitionInvoices.Add(invoice);

        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return CreatedAtAction(nameof(GetInvoice), new { id = invoice.InvoiceId }, invoice);
    }

    [HttpGet("invoices/{id:guid}")]
    public async Task<ActionResult<TuitionInvoice>> GetInvoice(Guid id)
    {
        var invoice = await _context.TuitionInvoices
            .Include(i => i.Payments)
            .FirstOrDefaultAsync(i => i.InvoiceId == id);
        return invoice is null ? NotFound() : Ok(invoice);
    }

    [HttpPost("invoices/{invoiceId:guid}/payments")]
    public async Task<ActionResult<PaymentTransaction>> CreatePayment(Guid invoiceId, [FromBody] PaymentCreateDto dto)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var invoice = await _context.TuitionInvoices
            .Include(i => i.BillingAccount)
            .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId);
        if (invoice is null) return NotFound("Invoice not found");

        var payment = new PaymentTransaction
        {
            PaymentId = Guid.NewGuid(),
            InvoiceId = invoiceId,
            Amount = dto.Amount
        };

        invoice.BillingAccount.Balance -= dto.Amount;
        _context.PaymentTransactions.Add(payment);

        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return CreatedAtAction(nameof(GetInvoice), new { id = invoiceId }, payment);
    }
}

public record BillingAccountCreateDto(Guid StudentId);
public record InvoiceCreateDto(decimal Amount);
public record PaymentCreateDto(decimal Amount);
