using System.Data;
using FleetNetworkApi.Data;
using FleetNetworkApi.DTOs;
using FleetNetworkApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetNetworkApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IncidentReportsController : ControllerBase
{
    private readonly FleetDbContext _db;
    public IncidentReportsController(FleetDbContext db) => _db = db;

    [HttpPost]
    public async Task<ActionResult<IncidentReport>> Create(IncidentCreateDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var vehicleExists = await _db.Vehicles.AnyAsync(v => v.Id == dto.VehicleId);
        if (!vehicleExists) return BadRequest("Vehicle not found");

        var incident = new IncidentReport
        {
            VehicleId = dto.VehicleId,
            RoutePlanId = dto.RoutePlanId,
            Summary = dto.Summary,
            EvidenceUri = dto.EvidenceUri,
            LockToken = Guid.NewGuid().ToString("N")
        };
        _db.IncidentReports.Add(incident);
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        return CreatedAtAction(nameof(Get), new { id = incident.Id }, incident);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IncidentReport>> Get(Guid id)
    {
        var incident = await _db.IncidentReports.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        return incident is null ? NotFound() : Ok(incident);
    }

    [HttpPost("{id:guid}/evidence")]
    public async Task<IActionResult> AppendEvidence(Guid id, IncidentAppendEvidenceDto dto)
    {
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var incident = await _db.IncidentReports.FirstOrDefaultAsync(i => i.Id == id);
        if (incident is null) return NotFound();
        if (!incident.RowVersion.SequenceEqual(dto.Version.RowVersionBytes))
            return Conflict("Concurrent update detected");

        incident.EvidenceUri = dto.EvidenceUri;
        incident.LockToken = dto.Version.LockToken ?? incident.LockToken;

        try
        {
            await _db.SaveChangesAsync();
            await tx.CommitAsync();
            return Ok(incident);
        }
        catch (DbUpdateConcurrencyException)
        {
            await tx.RollbackAsync();
            return Conflict("Ghost write prevented (rowversion mismatch)");
        }
    }
}
