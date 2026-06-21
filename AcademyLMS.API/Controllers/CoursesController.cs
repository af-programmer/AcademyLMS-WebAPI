using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademyLMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;

    public CoursesController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CourseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetAll(
        [FromQuery] int? teacherId,
        CancellationToken cancellationToken)
    {
        var courses = await _courseService.GetAllAsync(teacherId, cancellationToken);
        return Ok(courses);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var course = await _courseService.GetByIdAsync(id, cancellationToken);
        if (course is null)
        {
            return NotFound($"Course with id {id} was not found.");
        }

        return Ok(course);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDto>> Create(
        [FromBody] CourseCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _courseService.CreateAsync(createDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.CourseId }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CourseDto>> Update(
        [FromRoute] int id,
        [FromBody] CourseDto courseDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != courseDto.CourseId)
        {
            return BadRequest("Route id does not match course id in the request body.");
        }

        var updated = await _courseService.UpdateAsync(id, courseDto, cancellationToken);
        if (updated is null)
        {
            return NotFound($"Course with id {id} was not found.");
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var deleted = await _courseService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound($"Course with id {id} was not found.");
        }

        return Ok(new { message = $"Course with id {id} was deleted successfully." });
    }
}
