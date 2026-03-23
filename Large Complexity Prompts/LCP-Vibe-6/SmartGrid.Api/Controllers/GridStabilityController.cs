using Microsoft.AspNetCore.Mvc;
using SmartGrid.Api.Data;
using SmartGrid.Api.Models;

namespace SmartGrid.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GridStabilityController : ControllerBase
{
    private readonly InMemoryDatabase _db;

    public GridStabilityController(InMemoryDatabase db)
    {
        _db = db;
    }

    [HttpGet("state")]
    public IActionResult GetState()
    {
        return Ok(new
        {
            _db.GridStability,
            _db.SubstationMonitors,
            _db.LoadBalancers,
            _db.GridAlerts,
            _db.DemandResponseEvents
        });
    }

    [HttpPost("stability")]
    public ActionResult<GridStabilityIndex> AddStability(GridStabilityIndex index)
    {
        index.Id = _db.GridStability.Count == 0 ? 1 : _db.GridStability.Max(i => i.Id) + 1;
        index.Timestamp = index.Timestamp == default ? DateTime.UtcNow : index.Timestamp;
        _db.GridStability.Add(index);
        return CreatedAtAction(nameof(GetState), new { id = index.Id }, index);
    }

    [HttpGet("faults")]
    public IEnumerable<FaultRecord> GetFaults() => _db.FaultRecords;

    [HttpPost("faults")]
    public ActionResult<FaultRecord> AddFault(FaultRecord fault)
    {
        fault.Id = _db.FaultRecords.Count == 0 ? 1 : _db.FaultRecords.Max(f => f.Id) + 1;
        fault.OccurredAt = fault.OccurredAt == default ? DateTime.UtcNow : fault.OccurredAt;
        _db.FaultRecords.Add(fault);
        return CreatedAtAction(nameof(GetFaults), new { id = fault.Id }, fault);
    }

    [HttpGet("maintenance")]
    public IActionResult GetMaintenance()
    {
        return Ok(new { _db.MaintenanceSchedules, _db.CrewAssignments });
    }
}
