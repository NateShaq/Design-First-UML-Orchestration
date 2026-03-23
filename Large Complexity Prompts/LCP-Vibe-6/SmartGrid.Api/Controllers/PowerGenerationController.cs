using Microsoft.AspNetCore.Mvc;
using SmartGrid.Api.Data;
using SmartGrid.Api.Models;

namespace SmartGrid.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PowerGenerationController : ControllerBase
{
    private readonly InMemoryDatabase _db;

    public PowerGenerationController(InMemoryDatabase db)
    {
        _db = db;
    }

    [HttpGet("sources")]
    public IActionResult GetSources()
    {
        return Ok(new
        {
            _db.SolarFarms,
            _db.WindTurbines,
            _db.HydroPlants,
            _db.GeothermalPlants,
            _db.BiomassPlants,
            _db.BatteryStorages,
            _db.EnergyStorageSystems
        });
    }

    [HttpGet("generation-records")]
    public IEnumerable<PowerGenerationRecord> GetGenerationRecords() => _db.GenerationRecords;

    [HttpPost("generation-records")]
    public ActionResult<PowerGenerationRecord> AddGenerationRecord(PowerGenerationRecord record)
    {
        record.Id = _db.GenerationRecords.Count == 0 ? 1 : _db.GenerationRecords.Max(r => r.Id) + 1;
        record.Timestamp = record.Timestamp == default ? DateTime.UtcNow : record.Timestamp;
        _db.GenerationRecords.Add(record);
        return CreatedAtAction(nameof(GetGenerationRecords), new { id = record.Id }, record);
    }

    [HttpGet("ppa")]
    public IEnumerable<PowerPurchaseAgreement> GetPowerPurchaseAgreements() => _db.PowerPurchaseAgreements;

    [HttpPost("ppa")]
    public ActionResult<PowerPurchaseAgreement> AddPpa(PowerPurchaseAgreement ppa)
    {
        ppa.Id = _db.PowerPurchaseAgreements.Count == 0 ? 1 : _db.PowerPurchaseAgreements.Max(p => p.Id) + 1;
        _db.PowerPurchaseAgreements.Add(ppa);
        return CreatedAtAction(nameof(GetPowerPurchaseAgreements), new { id = ppa.Id }, ppa);
    }

    [HttpGet("forecast")]
    public IActionResult GetForecasts()
    {
        return Ok(new { Forecasts = _db.Forecasts, Accuracy = _db.ForecastAccuracies });
    }
}
