using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademyLMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeachersController : ControllerBase
{
    private readonly ITeacherService _teacherService;

    public TeachersController(ITeacherService teacherService)
    {
        _teacherService = teacherService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TeacherDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TeacherDto>>> GetAll(
        [FromQuery] string? department,
        CancellationToken cancellationToken)
    {
        var teachers = await _teacherService.GetAllAsync(department, cancellationToken);
        return Ok(teachers);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TeacherDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var teacher = await _teacherService.GetByIdAsync(id, cancellationToken);
        if (teacher is null)
        {
            return NotFound($"Teacher with id {id} was not found.");
        }

        return Ok(teacher);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TeacherDto>> Create(
        [FromBody] TeacherCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _teacherService.CreateAsync(createDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.TeacherId }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TeacherDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        var updated = await _teacherService.UpdateAsync(id, teacherDto, cancellationToken);
        if (updated is null)
        {
            return NotFound($"Teacher with id {id} was not found.");
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
        var deleted = await _teacherService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound($"Teacher with id {id} was not found.");
        }

        return Ok(new { message = $"Teacher with id {id} was deleted successfully." });
    }
}
