using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/smart-meters")]
public class SmartMetersController : ControllerBase
{
    private readonly GridDbContext _context;

    public SmartMetersController(GridDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SmartMeter>>> GetAll() =>
        await _context.SmartMeters.AsNoTracking().ToListAsync();

    [HttpGet("{meterId:guid}")]
    public async Task<ActionResult<SmartMeter>> Get(Guid meterId)
    {
        var meter = await _context.SmartMeters.AsNoTracking().FirstOrDefaultAsync(x => x.MeterId == meterId);
        return meter is null ? NotFound() : meter;
    }

    [HttpPost]
    public async Task<ActionResult<SmartMeter>> Create([FromBody] CreateSmartMeterRequest request)
    {
        var entity = new SmartMeter
        {
            MeterId = request.MeterId ?? Guid.NewGuid(),
            CustomerId = request.CustomerId,
            GridConnectionId = request.GridConnectionId
        };

        _context.SmartMeters.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { meterId = entity.MeterId }, entity);
    }
}
