using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/settlements")]
public class SettlementsController : ControllerBase
{
    private readonly GridDbContext _context;

    public SettlementsController(GridDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SettlementEngine>>> GetAll() =>
        await _context.SettlementEngines.AsNoTracking().ToListAsync();

    [HttpGet("{settlementId:guid}")]
    public async Task<ActionResult<SettlementEngine>> Get(Guid settlementId)
    {
        var entity = await _context.SettlementEngines.AsNoTracking().FirstOrDefaultAsync(x => x.SettlementId == settlementId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<SettlementEngine>> Create([FromBody] CreateSettlementRequest request)
    {
        var entity = new SettlementEngine
        {
            SettlementId = request.SettlementId ?? Guid.NewGuid(),
            Status = request.Status
        };

        _context.SettlementEngines.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { settlementId = entity.SettlementId }, entity);
    }
}
