using LcpUml6.Api.Contracts.Requests;
using LcpUml6.Api.Data;
using LcpUml6.Api.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml6.Api.Controllers;

[ApiController]
[Route("api/maintenance-schedules")]
public class MaintenanceSchedulesController : ControllerBase
{
    private readonly GridDbContext _context;

    public MaintenanceSchedulesController(GridDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaintenanceSchedule>>> GetAll() =>
        await _context.MaintenanceSchedules.AsNoTracking().ToListAsync();

    [HttpGet("{maintenanceScheduleId:guid}")]
    public async Task<ActionResult<MaintenanceSchedule>> Get(Guid maintenanceScheduleId)
    {
        var entity = await _context.MaintenanceSchedules.AsNoTracking().FirstOrDefaultAsync(x => x.MaintenanceScheduleId == maintenanceScheduleId);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<MaintenanceSchedule>> Create([FromBody] CreateMaintenanceScheduleRequest request)
    {
        var entity = new MaintenanceSchedule
        {
            MaintenanceScheduleId = request.MaintenanceScheduleId ?? Guid.NewGuid(),
            NextServiceDate = request.NextServiceDate,
            Notes = request.Notes
        };

        _context.MaintenanceSchedules.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { maintenanceScheduleId = entity.MaintenanceScheduleId }, entity);
    }
}
