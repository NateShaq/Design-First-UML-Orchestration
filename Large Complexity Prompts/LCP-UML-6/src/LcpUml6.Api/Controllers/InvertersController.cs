using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/inverters")]
public class InvertersController : ControllerBase
{
    private readonly GridDbContext _context;

    public InvertersController(GridDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Inverter>>> GetAll() =>
        await _context.Inverters.AsNoTracking().ToListAsync();

    [HttpGet("{inverterId:guid}")]
    public async Task<ActionResult<Inverter>> Get(Guid inverterId)
    {
        var entity = await _context.Inverters.AsNoTracking().FirstOrDefaultAsync(x => x.InverterId == inverterId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<Inverter>> Create([FromBody] CreateInverterRequest request)
    {
        var entity = new Inverter
        {
            InverterId = request.InverterId ?? Guid.NewGuid(),
            Model = request.Model,
            FirmwareVersion = request.FirmwareVersion
        };

        _context.Inverters.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { inverterId = entity.InverterId }, entity);
    }
}
