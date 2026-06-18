using AcademyLMS.BusinessLogic.DTOs;
using AcademyLMS.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace AcademyLMS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly IEnrollmentService _enrollmentService;
    private readonly ILogger<EnrollmentsController> _logger;

    public EnrollmentsController(IEnrollmentService enrollmentService, ILogger<EnrollmentsController> logger)
    {
        _enrollmentService = enrollmentService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<EnrollmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IReadOnlyList<EnrollmentDto>>> GetAll(
        [FromQuery] int? studentId,
        [FromQuery] int? courseId,
        CancellationToken cancellationToken)
    {
        try
        {
            var enrollments = await _enrollmentService.GetAllAsync(cancellationToken);

            if (studentId.HasValue)
            {
                enrollments = enrollments
                    .Where(e => e.StudentId == studentId.Value)
                    .ToList();
            }

            if (courseId.HasValue)
            {
                enrollments = enrollments
                    .Where(e => e.CourseId == courseId.Value)
                    .ToList();
            }

            return Ok(enrollments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving enrollments.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving enrollments.");
        }
    }

    [HttpGet("{studentId:int}/{courseId:int}")]
    [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EnrollmentDto>> GetById(
        [FromRoute] int studentId,
        [FromRoute] int courseId,
        CancellationToken cancellationToken)
    {
        try
        {
            var enrollment = await _enrollmentService.GetByIdAsync(studentId, courseId, cancellationToken);
            if (enrollment is null)
            {
                return NotFound($"Enrollment for student {studentId} and course {courseId} was not found.");
            }

            return Ok(enrollment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving enrollment for student {StudentId} and course {CourseId}.", studentId, courseId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving the enrollment.");
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<EnrollmentDto>> Create(
        [FromBody] EnrollmentCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var created = await _enrollmentService.CreateAsync(createDto, cancellationToken);
            if (created is null)
            {
                return NotFound(
                    $"Unable to create enrollment. Student {createDto.StudentId} or course {createDto.CourseId} was not found, or the student is already enrolled in this course.");
            }

            return CreatedAtAction(
                nameof(GetById),
                new { studentId = created.StudentId, courseId = created.CourseId },
                created);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating enrollment.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the enrollment.");
        }
    }

    [HttpPut("{studentId:int}/{courseId:int}")]
    [ProducesResponseType(typeof(EnrollmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        try
        {
            var updated = await _enrollmentService.UpdateAsync(studentId, courseId, enrollmentDto, cancellationToken);
            if (updated is null)
            {
                return NotFound($"Enrollment for student {studentId} and course {courseId} was not found.");
            }

            return Ok(updated);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating enrollment for student {StudentId} and course {CourseId}.", studentId, courseId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating the enrollment.");
        }
    }

    [HttpDelete("{studentId:int}/{courseId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> Delete(
        [FromRoute] int studentId,
        [FromRoute] int courseId,
        CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _enrollmentService.DeleteAsync(studentId, courseId, cancellationToken);
            if (!deleted)
            {
                return NotFound($"Enrollment for student {studentId} and course {courseId} was not found.");
            }

            return Ok(new { message = $"Enrollment for student {studentId} and course {courseId} was removed successfully." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting enrollment for student {StudentId} and course {CourseId}.", studentId, courseId);
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the enrollment.");
        }
    }
}
