using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademyLMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentService _studentService;

    public StudentsController(IStudentService studentService)
    {
        _studentService = studentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<StudentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StudentDto>>> GetAll(
        [FromQuery] string? email,
        CancellationToken cancellationToken)
    {
        var students = await _studentService.GetAllAsync(email, cancellationToken);
        return Ok(students);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StudentDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var student = await _studentService.GetByIdAsync(id, cancellationToken);
        if (student is null)
        {
            return NotFound($"Student with id {id} was not found.");
        }

        return Ok(student);
    }

    [HttpPost]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StudentDto>> Create(
        [FromBody] StudentCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _studentService.CreateAsync(createDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.StudentId }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(StudentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        var updated = await _studentService.UpdateAsync(id, studentDto, cancellationToken);
        if (updated is null)
        {
            return NotFound($"Student with id {id} was not found.");
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
        var deleted = await _studentService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound($"Student with id {id} was not found.");
        }

        return Ok(new { message = $"Student with id {id} was deleted successfully." });
    }
}
