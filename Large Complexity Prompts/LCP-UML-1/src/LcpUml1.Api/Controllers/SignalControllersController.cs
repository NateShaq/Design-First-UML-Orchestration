using System.Data;
using LcpUml1.Api.Data;
using LcpUml1.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SignalControllersController(SmartCityContext context) : ControllerBase
{
    private readonly SmartCityContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SignalController>>> GetAll(CancellationToken cancellationToken)
        => await _context.SignalControllers.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SignalController>> Get(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.SignalControllers.AsNoTracking()
            .FirstOrDefaultAsync(e => e.SignalControllerId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<SignalController>> Post(SignalController request, CancellationToken cancellationToken)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.SignalControllers.Add(request);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = request.SignalControllerId }, request);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(int id, SignalController request, CancellationToken cancellationToken)
    {
        if (id != request.SignalControllerId) return BadRequest("Id mismatch.");

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.Entry(request).Property(e => e.RowVersion).OriginalValue = request.RowVersion;
        _context.Entry(request).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync(cancellationToken);
            if (!await _context.SignalControllers.AnyAsync(e => e.SignalControllerId == id, cancellationToken))
                return NotFound();
            return Conflict("Concurrent update detected; refresh and retry.");
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var entity = await _context.SignalControllers.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();

        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
        _context.SignalControllers.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);
        return NoContent();
    }
}
