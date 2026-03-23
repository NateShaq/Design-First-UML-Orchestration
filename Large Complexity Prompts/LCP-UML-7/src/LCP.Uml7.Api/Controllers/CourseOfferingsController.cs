using System.Data;
using LCP.Uml7.Api.Data;
using LCP.Uml7.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LCP.Uml7.Api.Controllers;

[ApiController]
[Route("api/course-offerings")]
public class CourseOfferingsController : ControllerBase
{
    private readonly UniversityDbContext _context;

    public CourseOfferingsController(UniversityDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<ActionResult<CourseOffering>> Create([FromBody] CourseOfferingCreateDto dto)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var module = await _context.CourseModules.FindAsync(dto.ModuleId);
        if (module is null) return BadRequest("Module not found");

        var entity = new CourseOffering
        {
            OfferingId = Guid.NewGuid(),
            Term = dto.Term,
            Capacity = dto.Capacity,
            ModuleId = dto.ModuleId,
            FacultyId = dto.FacultyId
        };

        _context.CourseOfferings.Add(entity);
        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return CreatedAtAction(nameof(Get), new { id = entity.OfferingId }, entity);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseOffering>> Get(Guid id)
    {
        var offering = await _context.CourseOfferings
            .Include(o => o.Module)
            .Include(o => o.Enrollments)
            .FirstOrDefaultAsync(o => o.OfferingId == id);
        return offering is null ? NotFound() : Ok(offering);
    }

    [HttpPut("{id:guid}/capacity")]
    public async Task<IActionResult> UpdateCapacity(Guid id, [FromBody] CapacityUpdateDto dto)
    {
        await using var tx = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var offering = await _context.CourseOfferings
            .Include(o => o.Enrollments)
            .FirstOrDefaultAsync(o => o.OfferingId == id);
        if (offering is null) return NotFound();

        var enrolled = offering.Enrollments.Count;
        if (dto.Capacity < enrolled)
        {
            return BadRequest($"Cannot set capacity below current enrollment of {enrolled}.");
        }

        offering.Capacity = dto.Capacity;
        await _context.SaveChangesAsync();
        await tx.CommitAsync();

        return NoContent();
    }
}

public record CourseOfferingCreateDto(Guid ModuleId, string Term, int Capacity, Guid? FacultyId);
public record CapacityUpdateDto(int Capacity);
