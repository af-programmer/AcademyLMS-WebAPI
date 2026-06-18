using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademyLMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController : ControllerBase
{
    private readonly ICourseService _courseService;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(ICourseService courseService, ILogger<CoursesController> logger)
    {
        _courseService = courseService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CourseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<CourseDto>>> GetAll(
        [FromQuery] int? teacherId,
        CancellationToken cancellationToken)
    {
        try
        {
            var courses = await _courseService.GetAllAsync(cancellationToken);

            if (teacherId.HasValue)
            {
                courses = courses
                    .Where(c => c.TeacherId == teacherId.Value)
                    .ToList();
            }

            return Ok(courses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving courses.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving courses.");
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var course = await _courseService.GetByIdAsync(id, cancellationToken);
            if (course is null)
            {
                return NotFound($"Course with id {id} was not found.");
            }

            return Ok(course);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving course {CourseId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the course.");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CourseDto>> Create(
        [FromBody] CourseCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _courseService.CreateAsync(createDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.CourseId }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating course.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the course.");
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CourseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        try
        {
            var updated = await _courseService.UpdateAsync(id, courseDto, cancellationToken);
            if (updated is null)
            {
                return NotFound($"Course with id {id} was not found.");
            }

            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating course {CourseId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the course.");
        }
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _courseService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound($"Course with id {id} was not found.");
            }

            return Ok(new { message = $"Course with id {id} was deleted successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting course {CourseId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the course.");
        }
    }
}
