using GlobalEduERP.Data;
using GlobalEduERP.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlobalEduERP.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LearningController : ControllerBase
{
    private readonly InMemoryData _data;

    public LearningController(InMemoryData data)
    {
        _data = data;
    }

    [HttpGet("courses")]
    public ActionResult<IEnumerable<Course>> GetCourses() => _data.Courses;

    [HttpGet("curricula")]
    public ActionResult<IEnumerable<CourseCurriculum>> GetCurricula() => _data.Curricula;

    [HttpGet("research-projects")]
    public ActionResult<IEnumerable<ResearchProject>> GetResearchProjects() => _data.ResearchProjects;
}
