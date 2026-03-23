using LcpUml1.Api.Data;
using LcpUml1.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml1.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RoutesController(SmartCityContext context) : ControllerBase
{
    private readonly SmartCityContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RouteDim>>> GetAll(CancellationToken cancellationToken)
        => await _context.Routes.AsNoTracking().ToListAsync(cancellationToken);

    [HttpPost]
    public async Task<ActionResult<RouteDim>> Post(RouteDim route, CancellationToken cancellationToken)
    {
        _context.Routes.Add(route);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = route.RouteId }, route);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<RouteDim>> GetById(int id, CancellationToken cancellationToken)
    {
        var route = await _context.Routes.AsNoTracking().FirstOrDefaultAsync(r => r.RouteId == id, cancellationToken);
        return route is null ? NotFound() : route;
    }
}
