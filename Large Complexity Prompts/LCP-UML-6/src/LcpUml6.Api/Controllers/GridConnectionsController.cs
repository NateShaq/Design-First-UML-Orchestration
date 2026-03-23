using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/grid-connections")]
public class GridConnectionsController : ControllerBase
{
    private readonly GridDbContext _context;

    public GridConnectionsController(GridDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<GridConnection>>> GetAll() =>
        await _context.GridConnections.AsNoTracking().ToListAsync();

    [HttpGet("{gridConnectionId:guid}")]
    public async Task<ActionResult<GridConnection>> Get(Guid gridConnectionId)
    {
        var entity = await _context.GridConnections.AsNoTracking().FirstOrDefaultAsync(x => x.GridConnectionId == gridConnectionId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<GridConnection>> Create([FromBody] CreateGridConnectionRequest request)
    {
        var entity = new GridConnection
        {
            GridConnectionId = request.GridConnectionId ?? Guid.NewGuid(),
            Node = request.Node
        };

        _context.GridConnections.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { gridConnectionId = entity.GridConnectionId }, entity);
    }
}
