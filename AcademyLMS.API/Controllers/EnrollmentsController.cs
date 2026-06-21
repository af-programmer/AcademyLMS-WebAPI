using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademyLMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;

    public EnrollmentsController(IEnrollmentService enrollmentService)
    {
        _enrollmentService = enrollmentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EnrollmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<EnrollmentDto>>> GetAll(
        [FromQuery] int? studentId,
        [FromQuery] int? courseId,
        CancellationToken cancellationToken)
    {
        var enrollments = await _enrollmentService.GetAllAsync(studentId, courseId, cancellationToken);
        return Ok(enrollments);
    }

    [HttpGet("{studentId:int}/{courseId:int}")]
    [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EnrollmentDto>> GetById(
        [FromRoute] int studentId,
        [FromRoute] int courseId,
        CancellationToken cancellationToken)
    {
        var enrollment = await _enrollmentService.GetByIdAsync(studentId, courseId, cancellationToken);
        if (enrollment is null)
        {
            return NotFound($"Enrollment for student {studentId} and course {courseId} was not found.");
        }

        return Ok(enrollment);
    }

    [HttpPost]
    [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<EnrollmentDto>> Create(
        [FromBody] EnrollmentCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _enrollmentService.CreateAsync(createDto, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { studentId = created.StudentId, courseId = created.CourseId },
            created);
    }

    [HttpPut("{studentId:int}/{courseId:int}")]
    [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<EnrollmentDto>> Update(
        [FromRoute] int studentId,
        [FromRoute] int courseId,
        [FromBody] EnrollmentDto enrollmentDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (studentId != enrollmentDto.StudentId || courseId != enrollmentDto.CourseId)
        {
            return BadRequest("Route ids do not match student id and course id in the request body.");
        }

        var updated = await _enrollmentService.UpdateAsync(studentId, courseId, enrollmentDto, cancellationToken);
        if (updated is null)
        {
            return NotFound($"Enrollment for student {studentId} and course {courseId} was not found.");
        }

        return Ok(updated);
    }

    [HttpDelete("{studentId:int}/{courseId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(
        [FromRoute] int studentId,
        [FromRoute] int courseId,
        CancellationToken cancellationToken)
    {
        var deleted = await _enrollmentService.DeleteAsync(studentId, courseId, cancellationToken);
        if (!deleted)
        {
            return NotFound($"Enrollment for student {studentId} and course {courseId} was not found.");
        }

        return Ok(new { message = $"Enrollment for student {studentId} and course {courseId} was removed successfully." });
    }
}
