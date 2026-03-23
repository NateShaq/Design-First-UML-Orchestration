using LcpUml3.Api.Data;
using LcpUml3.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LabOrdersController(HospitalContext context) : ControllerBase
{
    private readonly HospitalContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LabOrder>>> GetAll(CancellationToken cancellationToken)
        => await _context.LabOrders.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{id}")]
    public async Task<ActionResult<LabOrder>> GetById(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.LabOrders.AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<LabOrder>> Post(LabOrder order, CancellationToken cancellationToken)
    {
        _context.LabOrders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, LabOrder order, CancellationToken cancellationToken)
    {
        if (id != order.OrderId) return BadRequest("Id mismatch.");
        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.LabOrders.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();
        _context.LabOrders.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
