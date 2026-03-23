using LcpUml3.Api.Data;
using LcpUml3.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LcpUml3.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController(HospitalContext context) : ControllerBase
{
    private readonly HospitalContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetAll(CancellationToken cancellationToken)
        => await _context.Patients.AsNoTracking().ToListAsync(cancellationToken);

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetById(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.Patients.AsNoTracking().FirstOrDefaultAsync(p => p.PatientId == id, cancellationToken);
        return entity is null ? NotFound() : entity;
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> Post(Patient patient, CancellationToken cancellationToken)
    {
        _context.Patients.Add(patient);
        await _context.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = patient.PatientId }, patient);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(string id, Patient patient, CancellationToken cancellationToken)
    {
        if (id != patient.PatientId) return BadRequest("Id mismatch.");
        _context.Entry(patient).State = EntityState.Modified;
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken cancellationToken)
    {
        var entity = await _context.Patients.FindAsync(new object[] { id }, cancellationToken);
        if (entity is null) return NotFound();
        _context.Patients.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
