using LCP.Uml7.Api.Data;
using LCP.Uml7.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LCP.Uml7.Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly UniversityDbContext _context;

    public StudentsController(UniversityDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Student>> Get(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        return student is null ? NotFound() : Ok(student);
    }

    [HttpPost("undergraduates")]
    public async Task<ActionResult<Student>> CreateUndergraduate([FromBody] StudentCreateDto dto)
    {
        var entity = new Undergraduate
        {
            StudentId = Guid.NewGuid(),
            LegalName = dto.LegalName,
            Email = dto.Email,
            Revision = dto.Revision
        };
        _context.Undergraduates.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = entity.StudentId }, entity);
    }

    [HttpPost("postgraduates")]
    public async Task<ActionResult<Student>> CreatePostgraduate([FromBody] StudentCreateDto dto)
    {
        var entity = new Postgraduate
        {
            StudentId = Guid.NewGuid(),
            LegalName = dto.LegalName,
            Email = dto.Email,
            Revision = dto.Revision
        };
        _context.Postgraduates.Add(entity);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(Get), new { id = entity.StudentId }, entity);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] StudentUpdateDto dto)
    {
        var entity = await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id);
        if (entity is null) return NotFound();

        entity.LegalName = dto.LegalName;
        entity.Email = dto.Email;
        entity.Revision = dto.Revision;
        _context.Entry(entity).Property(e => e.RowVersion).OriginalValue = dto.RowVersion;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("Revision mismatch – concurrent update detected.");
        }

        return NoContent();
    }
}

public record StudentCreateDto(string LegalName, string Email, int Revision);

public record StudentUpdateDto(string LegalName, string Email, int Revision, byte[] RowVersion);
