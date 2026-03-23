using Microsoft.AspNetCore.Mvc;
using SmartGrid.Api.Data;
using SmartGrid.Api.Models;

namespace SmartGrid.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomerMeteringController : ControllerBase
{
    private readonly InMemoryDatabase _db;

    public CustomerMeteringController(InMemoryDatabase db)
    {
        _db = db;
    }

    [HttpGet("customers")]
    public IEnumerable<CustomerAccount> GetCustomers() => _db.CustomerAccounts;

    [HttpPost("customers")]
    public ActionResult<CustomerAccount> AddCustomer(CustomerAccount account)
    {
        account.Id = _db.CustomerAccounts.Count == 0 ? 1 : _db.CustomerAccounts.Max(c => c.Id) + 1;
        _db.CustomerAccounts.Add(account);
        return CreatedAtAction(nameof(GetCustomers), new { id = account.Id }, account);
    }

    [HttpGet("meters")]
    public IActionResult GetMeters()
    {
        return Ok(new { _db.SmartMeters, _db.MeterReadings });
    }

    [HttpPost("meter-readings")]
    public ActionResult<MeterReading> AddReading(MeterReading reading)
    {
        reading.Id = _db.MeterReadings.Count == 0 ? 1 : _db.MeterReadings.Max(r => r.Id) + 1;
        reading.ReadAt = reading.ReadAt == default ? DateTime.UtcNow : reading.ReadAt;
        _db.MeterReadings.Add(reading);
        return CreatedAtAction(nameof(GetMeters), new { id = reading.Id }, reading);
    }

    [HttpGet("billing")]
    public IActionResult GetBilling()
    {
        return Ok(new
        {
            _db.BillingCycles,
            _db.UsageBillings,
            _db.Invoices,
            _db.Payments,
            _db.TariffPlans
        });
    }

    [HttpPost("usage-billing")]
    public ActionResult<UsageBilling> AddUsageBilling(UsageBilling billing)
    {
        billing.Id = _db.UsageBillings.Count == 0 ? 1 : _db.UsageBillings.Max(b => b.Id) + 1;
        _db.UsageBillings.Add(billing);
        return CreatedAtAction(nameof(GetBilling), new { id = billing.Id }, billing);
    }

    [HttpGet("outages")]
    public IEnumerable<OutageTicket> GetOutages() => _db.OutageTickets;
}
