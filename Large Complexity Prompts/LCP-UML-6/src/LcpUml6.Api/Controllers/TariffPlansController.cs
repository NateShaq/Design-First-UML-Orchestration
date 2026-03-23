using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/tariff-plans")]
public class TariffPlansController : ControllerBase
{
    private readonly GridDbContext _context;

    public TariffPlansController(GridDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TariffPlan>>> GetAll() =>
        await _context.TariffPlans.AsNoTracking().ToListAsync();

    [HttpGet("{tariffPlanId:guid}")]
    public async Task<ActionResult<TariffPlan>> Get(Guid tariffPlanId)
    {
        var entity = await _context.TariffPlans.AsNoTracking().FirstOrDefaultAsync(x => x.TariffPlanId == tariffPlanId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<TariffPlan>> Create([FromBody] CreateTariffPlanRequest request)
    {
        var entity = new TariffPlan
        {
            TariffPlanId = request.TariffPlanId ?? Guid.NewGuid(),
            Name = request.Name,
            RateStructure = request.RateStructure
        };

        _context.TariffPlans.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { tariffPlanId = entity.TariffPlanId }, entity);
    }
}
