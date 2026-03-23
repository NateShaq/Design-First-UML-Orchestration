using System.Data;
using LcpUml3.Api.Data;
using LcpUml3.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InsuranceClaimsController(HospitalContext context) : ControllerBase
{
    private readonly HospitalContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<InsuranceClaim>>> GetAll(CancellationToken cancellationToken)
        => await _context.InsuranceClaims.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{id}")]
    public async Task<ActionResult<InsuranceClaim>> GetById(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.InsuranceClaims.AsNoTracking().FirstOrDefaultAsync(e => e.ClaimId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<InsuranceClaim>> Post(InsuranceClaim claim, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.InsuranceClaims.Add(claim);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = claim.ClaimId }, claim);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, InsuranceClaim claim, CancellationToken cancellationToken)
    {
        if (id != claim.ClaimId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(claim).Property(e => e.RowVersion).OriginalValue = claim.RowVersion;
        _context.Entry(claim).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.InsuranceClaims.AnyAsync(e => e.ClaimId == id, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected; refresh and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.InsuranceClaims.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.InsuranceClaims.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
