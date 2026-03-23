using Api.Domain;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    private readonly IGovernmentDataStore _store;

    public IdentityController(IGovernmentDataStore store)
    {
        _store = store;
    }

    [HttpGet("citizens")]
    public ActionResult<IEnumerable<Citizen>> GetCitizens() => Ok(_store.Citizens);

    [HttpPost("citizens")]
    public ActionResult<Citizen> CreateCitizen([FromBody] Citizen citizen)
    {
        citizen.Id = Guid.NewGuid();
        _store.Citizens.Add(citizen);
        _store.AuditTrails.Add(new AuditTrail { ActorId = citizen.Id, Action = "CitizenCreated", Details = citizen.FullName });
        return CreatedAtAction(nameof(GetCitizens), new { id = citizen.Id }, citizen);
    }

    [HttpPost("registry")]
    public ActionResult<IdentityRecord> AddIdentity([FromBody] IdentityRecord record)
    {
        record.Id = Guid.NewGuid();
        _store.Identities.Add(record);
        _store.Registry.Records.Add(record);
        _store.AuditTrails.Add(new AuditTrail { ActorId = record.CitizenId, Action = "IdentityRegistered" });
        return CreatedAtAction(nameof(GetRegistry), new { id = record.Id }, record);
    }

    [HttpGet("registry")]
    public ActionResult<IdentityRegistry> GetRegistry() => Ok(_store.Registry);

    [HttpPost("biometrics")]
    public ActionResult<BiometricData> CaptureBiometric([FromBody] BiometricData biometric)
    {
        biometric.Id = Guid.NewGuid();
        _store.Biometrics.Add(biometric);
        _store.AuditTrails.Add(new AuditTrail { ActorId = biometric.CitizenId, Action = "BiometricCaptured", Details = biometric.Modality });
        return CreatedAtAction(nameof(GetBiometrics), new { id = biometric.Id }, biometric);
    }

    [HttpGet("biometrics/{citizenId:guid}")]
    public ActionResult<IEnumerable<BiometricData>> GetBiometrics(Guid citizenId) =>
        Ok(_store.Biometrics.Where(b => b.CitizenId == citizenId));
}
