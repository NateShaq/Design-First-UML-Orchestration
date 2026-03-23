using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/weather-stations")]
public class WeatherStationsController : ControllerBase
{
    private readonly GridDbContext _context;

    public WeatherStationsController(GridDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WeatherStation>>> GetAll() =>
        await _context.WeatherStations.AsNoTracking().ToListAsync();

    [HttpGet("{weatherStationId:guid}")]
    public async Task<ActionResult<WeatherStation>> Get(Guid weatherStationId)
    {
        var entity = await _context.WeatherStations.AsNoTracking().FirstOrDefaultAsync(x => x.WeatherStationId == weatherStationId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<WeatherStation>> Create([FromBody] CreateWeatherStationRequest request)
    {
        var entity = new WeatherStation
        {
            WeatherStationId = request.WeatherStationId ?? Guid.NewGuid(),
            Location = request.Location
        };

        _context.WeatherStations.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { weatherStationId = entity.WeatherStationId }, entity);
    }
}
