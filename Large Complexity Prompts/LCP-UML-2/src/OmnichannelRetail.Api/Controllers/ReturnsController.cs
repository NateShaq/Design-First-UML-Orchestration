using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OmnichannelRetail.Api.Data;
using OmnichannelRetail.Api.Models;

namespace OmnichannelRetail.Api.Controllers;

[ApiController]
[Route("api/returns")]
public class ReturnsController : ControllerBase
{
    private readonly RetailDbContext _context;
    public ReturnsController(RetailDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<ReturnAuthorization>> Create(ReturnAuthorization rma)
    {
        if (await _context.ReturnAuthorizations.AnyAsync(r => r.RmaNumber == rma.RmaNumber))
        {
            return Conflict($"RMA {rma.RmaNumber} already exists.");
        }

        _context.ReturnAuthorizations.Add(rma);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { rmaNumber = rma.RmaNumber }, rma);
    }

    [HttpGet("{rmaNumber}")]
    public async Task<ActionResult<ReturnAuthorization>> Get(string rmaNumber)
    {
        var rma = await _context.ReturnAuthorizations.FindAsync(rmaNumber);
        if (rma == null) return NotFound();
        return rma;
    }
}
