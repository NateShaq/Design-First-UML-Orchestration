using LcpUml1.Api.Data;
using LcpUml1.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CalendarController(SmartCityContext context) : ControllerBase
{
    private readonly SmartCityContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarDim>>> GetAll(CancellationToken cancellationToken)
        => await _context.Calendar.AsNoTracking().ToListAsync(cancellationToken);

    [HttpPost]
    public async Task<ActionResult<CalendarDim>> Post(CalendarDim date, CancellationToken cancellationToken)
    {
        _context.Calendar.Add(date);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = date.DateId }, date);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CalendarDim>> GetById(int id, CancellationToken cancellationToken)
    {
        var date = await _context.Calendar.AsNoTracking().FirstOrDefaultAsync(c => c.DateId == id, cancellationToken);
        return date is null ? NotFound() : date;
    }
}
