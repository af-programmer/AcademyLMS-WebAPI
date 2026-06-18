using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademyLMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly ILogger<TeachersController> _logger;

    public TeachersController(ITeacherService teacherService, ILogger<TeachersController> logger)
    {
        _teacherService = teacherService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TeacherDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<TeacherDto>>> GetAll(
        [FromQuery] string? department,
        CancellationToken cancellationToken)
    {
        try
        {
            var teachers = await _teacherService.GetAllAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(department))
            {
                teachers = teachers
                    .Where(t => t.Department != null &&
                                t.Department.Equals(department, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Ok(teachers);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving teachers.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving teachers.");
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TeacherDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var teacher = await _teacherService.GetByIdAsync(id, cancellationToken);
            if (teacher is null)
            {
                return NotFound($"Teacher with id {id} was not found.");
            }

            return Ok(teacher);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving teacher {TeacherId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the teacher.");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TeacherDto>> Create(
        [FromBody] TeacherCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _teacherService.CreateAsync(createDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.TeacherId }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating teacher.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the teacher.");
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TeacherDto>> Update(
        [FromRoute] int id,
        [FromBody] TeacherDto teacherDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != teacherDto.TeacherId)
        {
            return BadRequest("Route id does not match teacher id in the request body.");
        }

        try
        {
            var updated = await _teacherService.UpdateAsync(id, teacherDto, cancellationToken);
            if (updated is null)
            {
                return NotFound($"Teacher with id {id} was not found.");
            }

            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating teacher {TeacherId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the teacher.");
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
            var deleted = await _teacherService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound($"Teacher with id {id} was not found.");
            }

            return Ok(new { message = $"Teacher with id {id} was deleted successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting teacher {TeacherId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the teacher.");
        }
    }
}
