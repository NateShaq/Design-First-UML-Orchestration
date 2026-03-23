using LcpUml5.Api.Contracts.Requests;
using LcpUml5.Api.Domain.Entities;
using LcpUml5.Api.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml5.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TaxDocumentsController : ControllerBase
{
    private readonly BankingDbContext _db;

    public TaxDocumentsController(BankingDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    public async Task<ActionResult<TaxDocument>> Create(CreateTaxDocumentRequest request)
    {
        var customer = await _db.CustomerProfiles.FindAsync(request.CustomerProfileId);
        if (customer is null) return NotFound("Customer not found");

        var document = new TaxDocument
        {
            CustomerProfileId = request.CustomerProfileId,
            TaxYear = request.TaxYear,
            IdempotencyKey = request.IdempotencyKey
        };

        _db.TaxDocuments.Add(document);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = document.Id }, document);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaxDocument>> GetById(int id)
    {
        var doc = await _db.TaxDocuments.FindAsync(id);
        return doc is null ? NotFound() : Ok(doc);
    }
}
