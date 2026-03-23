using System.Data;
using LcpUml5.Api.Contracts.Requests;
using LcpUml5.Api.Domain.Entities;
using LcpUml5.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace LcpUml5.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly BankingDbContext _db;

    public AccountsController(BankingDbContext db)
    {
        _db = db;
    }

    [HttpPost("savings")]
    public async Task<ActionResult<SavingsAccount>> CreateSavings(CreateSavingsAccountRequest request)
    {
        var customer = await _db.CustomerProfiles.FindAsync(request.CustomerProfileId);
        if (customer is null) return NotFound("Customer not found");

        var account = new SavingsAccount
        {
            CustomerProfileId = request.CustomerProfileId,
            AccountNumber = request.AccountNumber,
            Balance = request.InitialDeposit,
            InterestRate = request.InterestRate,
            Currency = request.Currency,
            IdempotencyKey = request.IdempotencyKey
        };

        _db.SavingsAccounts.Add(account);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetSavingsById), new { id = account.Id }, account);
    }

    [HttpGet("savings/{id:int}")]
    public async Task<ActionResult<SavingsAccount>> GetSavingsById(int id)
    {
        var account = await _db.SavingsAccounts.FindAsync(id);
        return account is null ? NotFound() : Ok(account);
    }

    [HttpPost("savings/{id:int}/credit")]
    public async Task<ActionResult> Credit(int id, MoneyMovementRequest request)
    {
        return await ExecuteAtomicAsync(async () =>
        {
            var account = await _db.SavingsAccounts.FirstOrDefaultAsync(a => a.Id == id);
            if (account is null) return NotFound();

            account.Balance += request.Amount;
            account.Version += 1;
            account.IdempotencyKey ??= request.IdempotencyKey;

            await _db.SaveChangesAsync();
            return Ok(new { account.Balance, account.Version, account.RowVersion });
        });
    }

    [HttpPost("savings/{id:int}/debit")]
    public async Task<ActionResult> Debit(int id, MoneyMovementRequest request)
    {
        return await ExecuteAtomicAsync(async () =>
        {
            var account = await _db.SavingsAccounts.FirstOrDefaultAsync(a => a.Id == id);
            if (account is null) return NotFound();
            if (account.Balance < request.Amount) return BadRequest("Insufficient funds");

            account.Balance -= request.Amount;
            account.Version += 1;
            account.IdempotencyKey ??= request.IdempotencyKey;

            await _db.SaveChangesAsync();
            return Ok(new { account.Balance, account.Version, account.RowVersion });
        });
    }

    [HttpPost("crypto")]
    public async Task<ActionResult<CryptoWallet>> CreateWallet(CreateCryptoWalletRequest request)
    {
        var customer = await _db.CustomerProfiles.FindAsync(request.CustomerProfileId);
        if (customer is null) return NotFound("Customer not found");

        var wallet = new CryptoWallet
        {
            CustomerProfileId = request.CustomerProfileId,
            AccountNumber = request.AccountNumber,
            Chain = request.Chain,
            PublicAddress = request.PublicAddress,
            Currency = request.Currency,
            IdempotencyKey = request.IdempotencyKey
        };

        _db.CryptoWallets.Add(wallet);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetWalletById), new { id = wallet.Id }, wallet);
    }

    [HttpGet("crypto/{id:int}")]
    public async Task<ActionResult<CryptoWallet>> GetWalletById(int id)
    {
        var wallet = await _db.CryptoWallets.FindAsync(id);
        return wallet is null ? NotFound() : Ok(wallet);
    }

    private async Task<ActionResult> ExecuteAtomicAsync(Func<Task<ActionResult>> action)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        try
        {
            var result = await action();
            if (result is IStatusCodeActionResult { StatusCode: >= 400 })
            {
                await tx.RollbackAsync();
                return result;
            }

            await tx.CommitAsync();
            return result;
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync();
            return Conflict("Concurrent update detected; retry with latest RowVersion.");
        }
    }
}
