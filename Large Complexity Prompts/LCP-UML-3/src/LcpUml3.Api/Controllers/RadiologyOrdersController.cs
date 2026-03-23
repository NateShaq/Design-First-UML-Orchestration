using LcpUml3.Api.Data;
using LcpUml3.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RadiologyOrdersController(HospitalContext context) : ControllerBase
{
    private readonly HospitalContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RadiologyOrder>>> GetAll(CancellationToken cancellationToken)
        => await _context.RadiologyOrders.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{id}")]
    public async Task<ActionResult<RadiologyOrder>> GetById(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.RadiologyOrders.AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<RadiologyOrder>> Post(RadiologyOrder order, CancellationToken cancellationToken)
    {
        _context.RadiologyOrders.Add(order);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = order.OrderId }, order);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, RadiologyOrder order, CancellationToken cancellationToken)
    {
        if (id != order.OrderId) return BadRequest("Id mismatch.");
        _context.Entry(order).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.RadiologyOrders.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();
        _context.RadiologyOrders.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
