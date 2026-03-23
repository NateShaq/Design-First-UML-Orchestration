using LcpUml3.Api.Data;
using LcpUml3.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientRecordsController(HospitalContext context) : ControllerBase
{
    private readonly HospitalContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PatientRecord>>> GetAll(CancellationToken cancellationToken)
        => await _context.PatientRecords.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{id}")]
    public async Task<ActionResult<PatientRecord>> GetById(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.PatientRecords.AsNoTracking().FirstOrDefaultAsync(pr => pr.RecordId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<PatientRecord>> Post(PatientRecord record, CancellationToken cancellationToken)
    {
        _context.PatientRecords.Add(record);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = record.RecordId }, record);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, PatientRecord record, CancellationToken cancellationToken)
    {
        if (id != record.RecordId) return BadRequest("Id mismatch.");
        _context.Entry(record).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.PatientRecords.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();
        _context.PatientRecords.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
