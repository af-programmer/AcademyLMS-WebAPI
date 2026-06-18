using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademyLMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IStudentService studentService, ILogger<StudentsController> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StudentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<StudentDto>>> GetAll(
        [FromQuery] string? email,
        CancellationToken cancellationToken)
    {
        try
        {
            var students = await _studentService.GetAllAsync(cancellationToken);

            if (!string.IsNullOrWhiteSpace(email))
            {
                students = students
                    .Where(s => s.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return Ok(students);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving students.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving students.");
        }
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        try
        {
            var student = await _studentService.GetByIdAsync(id, cancellationToken);
            if (student is null)
            {
                return NotFound($"Student with id {id} was not found.");
            }

            return Ok(student);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving student {StudentId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the student.");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDto>> Create(
        [FromBody] StudentCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _studentService.CreateAsync(createDto, cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = created.StudentId }, created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating student.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the student.");
        }
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StudentDto>> Update(
        [FromRoute] int id,
        [FromBody] StudentDto studentDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != studentDto.StudentId)
        {
            return BadRequest("Route id does not match student id in the request body.");
        }

        try
        {
            var updated = await _studentService.UpdateAsync(id, studentDto, cancellationToken);
            if (updated is null)
            {
                return NotFound($"Student with id {id} was not found.");
            }

            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating student {StudentId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the student.");
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
            var deleted = await _studentService.DeleteAsync(id, cancellationToken);
            if (!deleted)
            {
                return NotFound($"Student with id {id} was not found.");
            }

            return Ok(new { message = $"Student with id {id} was deleted successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting student {StudentId}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the student.");
        }
    }
}
