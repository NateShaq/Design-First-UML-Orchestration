using Api.Domain;
using Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/credentials")]
public class CredentialsController : ControllerBase
{
    private readonly IGovernmentDataStore _store;

    public CredentialsController(IGovernmentDataStore store)
    {
        _store = store;
    }

    [HttpPost("passport/applications")]
    public ActionResult<PassportApplication> SubmitPassport([FromBody] PassportApplication application)
    {
        application.Id = Guid.NewGuid();
        _store.PassportApplications.Add(application);
        _store.AuditTrails.Add(new AuditTrail { ActorId = application.CitizenId, Action = "PassportApplied" });
        return CreatedAtAction(nameof(GetPassportApplications), new { id = application.Id }, application);
    }

    [HttpGet("passport/applications")]
    public ActionResult<IEnumerable<PassportApplication>> GetPassportApplications() => Ok(_store.PassportApplications);

    [HttpPost("passport/issue")]
    public ActionResult<Passport> IssuePassport([FromBody] Passport passport)
    {
        passport.Id = Guid.NewGuid();
        _store.Passports.Add(passport);
        _store.AuditTrails.Add(new AuditTrail { ActorId = passport.CitizenId, Action = "PassportIssued", Details = passport.PassportNumber });
        return CreatedAtAction(nameof(GetPassports), new { id = passport.Id }, passport);
    }

    [HttpGet("passport")]
    public ActionResult<IEnumerable<Passport>> GetPassports() => Ok(_store.Passports);

    [HttpPost("drivers-license/applications")]
    public ActionResult<LicenseApplication> ApplyForLicense([FromBody] LicenseApplication application)
    {
        application.Id = Guid.NewGuid();
        _store.LicenseApplications.Add(application);
        _store.AuditTrails.Add(new AuditTrail { ActorId = application.CitizenId, Action = "DriverLicenseApplied" });
        return CreatedAtAction(nameof(GetLicenseApplications), new { id = application.Id }, application);
    }

    [HttpGet("drivers-license/applications")]
    public ActionResult<IEnumerable<LicenseApplication>> GetLicenseApplications() => Ok(_store.LicenseApplications);

    [HttpPost("drivers-license/issue")]
    public ActionResult<DriversLicense> IssueLicense([FromBody] DriversLicense license)
    {
        license.Id = Guid.NewGuid();
        _store.Licenses.Add(license);
        _store.AuditTrails.Add(new AuditTrail { ActorId = license.CitizenId, Action = "DriverLicenseIssued", Details = license.LicenseNumber });
        return CreatedAtAction(nameof(GetLicenses), new { id = license.Id }, license);
    }

    [HttpGet("drivers-license")]
    public ActionResult<IEnumerable<DriversLicense>> GetLicenses() => Ok(_store.Licenses);
}
