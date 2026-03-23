using System.Data;
using LcpUml3.Api.Data;
using LcpUml3.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BillingInvoicesController(HospitalContext context) : ControllerBase
{
    private readonly HospitalContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<BillingInvoice>>> GetAll(CancellationToken cancellationToken)
        => await _context.BillingInvoices.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{id}")]
    public async Task<ActionResult<BillingInvoice>> GetById(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.BillingInvoices.AsNoTracking().FirstOrDefaultAsync(e => e.InvoiceId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<BillingInvoice>> Post(BillingInvoice invoice, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.BillingInvoices.Add(invoice);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = invoice.InvoiceId }, invoice);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, BillingInvoice invoice, CancellationToken cancellationToken)
    {
        if (id != invoice.InvoiceId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(invoice).Property(e => e.RowVersion).OriginalValue = invoice.RowVersion;
        _context.Entry(invoice).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.BillingInvoices.AnyAsync(e => e.InvoiceId == id, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected; refresh and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.BillingInvoices.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.BillingInvoices.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
