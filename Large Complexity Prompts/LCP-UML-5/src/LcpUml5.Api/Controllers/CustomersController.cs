using LcpUml5.Api.Contracts.Requests;
using LcpUml5.Api.Domain.Entities;
using LcpUml5.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml5.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly BankingDbContext _db;

    public CustomersController(BankingDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<CustomerProfile>> Create(CreateCustomerProfileRequest request)
    {
        var profile = new CustomerProfile
        {
            CustomerId = request.CustomerId,
            Segment = request.Segment,
            RiskLevel = request.RiskLevel
        };

        _db.CustomerProfiles.Add(profile);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = profile.Id }, profile);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CustomerProfile>> GetById(int id)
    {
        var profile = await _db.CustomerProfiles
            .Include(p => p.Accounts)
            .Include(p => p.Portfolios)
            .FirstOrDefaultAsync(p => p.Id == id);

        return profile is null ? NotFound() : Ok(profile);
    }
}
