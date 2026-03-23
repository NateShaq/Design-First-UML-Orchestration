using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/forecast-services")]
public class ForecastServicesController : ControllerBase
{
    private readonly GridDbContext _context;

    public ForecastServicesController(GridDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ForecastService>>> GetAll() =>
        await _context.ForecastServices.AsNoTracking().ToListAsync();

    [HttpGet("{forecastServiceId:guid}")]
    public async Task<ActionResult<ForecastService>> Get(Guid forecastServiceId)
    {
        var entity = await _context.ForecastServices.AsNoTracking().FirstOrDefaultAsync(x => x.ForecastServiceId == forecastServiceId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<ForecastService>> Create([FromBody] CreateForecastServiceRequest request)
    {
        var entity = new ForecastService
        {
            ForecastServiceId = request.ForecastServiceId ?? Guid.NewGuid(),
            Provider = request.Provider,
            WeatherStationId = request.WeatherStationId
        };

        _context.ForecastServices.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { forecastServiceId = entity.ForecastServiceId }, entity);
    }
}
