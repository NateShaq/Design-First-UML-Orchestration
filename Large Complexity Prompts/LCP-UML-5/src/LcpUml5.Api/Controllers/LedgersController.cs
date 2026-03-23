using System.Data;
using LcpUml5.Api.Contracts.Requests;
using LcpUml5.Api.Domain.Entities;
using LcpUml5.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml5.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LedgersController : ControllerBase
{
    private readonly BankingDbContext _db;

    public LedgersController(BankingDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<TransactionLedger>> Create([FromBody] string ledgerCode)
    {
        var ledger = new TransactionLedger { LedgerId = ledgerCode };
        _db.TransactionLedgers.Add(ledger);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = ledger.Id }, ledger);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TransactionLedger>> GetById(int id)
    {
        var ledger = await _db.TransactionLedgers
            .Include(l => l.Entries)
            .FirstOrDefaultAsync(l => l.Id == id);
        return ledger is null ? NotFound() : Ok(ledger);
    }

    [HttpPost("{id:int}/entries")]
    public async Task<ActionResult> PostEntry(int id, PostLedgerEntryRequest request)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var ledger = await _db.TransactionLedgers.FirstOrDefaultAsync(l => l.Id == id);
            if (ledger is null) return NotFound();

            var entry = new LedgerEntry
            {
                TransactionLedgerId = ledger.Id,
                Amount = request.Amount,
                EntryType = request.EntryType,
                Reference = request.Reference
            };

            ledger.Version += 1;
            ledger.IdempotencyKey ??= request.IdempotencyKey;

            _db.LedgerEntries.Add(entry);
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return Ok(new { ledger.Version, ledger.RowVersion, entry.Id });
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync();
            return Conflict("Concurrent ledger mutation detected.");
        }
    }
}
