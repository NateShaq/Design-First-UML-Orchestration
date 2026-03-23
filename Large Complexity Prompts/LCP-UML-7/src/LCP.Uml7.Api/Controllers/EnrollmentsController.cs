using System.Data;
using LCP.Uml7.Api.Data;
using LCP.Uml7.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LCP.Uml7.Api.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController : ControllerBase
{
    private readonly UniversityDbContext _context;

    public EnrollmentsController(UniversityDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<Enrollment>> Enroll([FromBody] EnrollmentCreateDto dto)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var offering = await _context.CourseOfferings
            .Include(o => o.Enrollments)
            .FirstOrDefaultAsync(o => o.OfferingId == dto.OfferingId);
        if (offering is null) return BadRequest("Offering not found");

        var studentExists = await _context.Students.AnyAsync(s => s.StudentId == dto.StudentId);
        if (!studentExists) return BadRequest("Student not found");

        if (offering.Enrollments.Count >= offering.Capacity)
        {
            return Conflict("Capacity reached; cannot enroll.");
        }

        var enrollment = new Enrollment
        {
            EnrollmentId = Guid.NewGuid(),
            StudentId = dto.StudentId,
            OfferingId = dto.OfferingId,
            Status = "Active"
        };

        _context.Enrollments.Add(enrollment);
        _context.StudentCourseOfferings.Add(new StudentCourseOffering
        {
            StudentId = dto.StudentId,
            OfferingId = dto.OfferingId
        });

        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return CreatedAtAction(nameof(Get), new { id = enrollment.EnrollmentId }, enrollment);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Enrollment>> Get(Guid id)
    {
        var enrollment = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Offering)
            .FirstOrDefaultAsync(e => e.EnrollmentId == id);
        return enrollment is null ? NotFound() : Ok(enrollment);
    }

    [HttpPut("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] EnrollmentStatusDto dto)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment is null) return NotFound();

        enrollment.Status = dto.Status;
        await _context.SaveChangesAsync();
        await tx.CommitAsync();
        return NoContent();
    }
}

public record EnrollmentCreateDto(Guid StudentId, Guid OfferingId);
public record EnrollmentStatusDto(string Status);
