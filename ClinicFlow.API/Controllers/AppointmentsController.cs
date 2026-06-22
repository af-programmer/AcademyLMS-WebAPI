using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<AppointmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<AppointmentDto>>> GetAll(
        [FromQuery] int? patientId,
        [FromQuery] int? treatmentId,
        CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAllAsync(patientId, treatmentId, cancellationToken);
        return Ok(appointments);
    }

    [HttpGet("{patientId:int}/{treatmentId:int}")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> GetById(
        [FromRoute] int patientId,
        [FromRoute] int treatmentId,
        CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.GetByIdAsync(patientId, treatmentId, cancellationToken);
        if (appointment is null)
        {
            return NotFound($"Appointment for patient {patientId} and treatment {treatmentId} was not found.");
        }

        return Ok(appointment);
    }

    [HttpPost]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<AppointmentDto>> Create(
        [FromBody] AppointmentCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _appointmentService.CreateAsync(createDto, cancellationToken);
        return CreatedAtAction(
            nameof(GetById),
            new { patientId = created.PatientId, treatmentId = created.TreatmentId },
            created);
    }

    [HttpPut("{patientId:int}/{treatmentId:int}")]
    [ProducesResponseType(typeof(AppointmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AppointmentDto>> Update(
        [FromRoute] int patientId,
        [FromRoute] int treatmentId,
        [FromBody] AppointmentDto appointmentDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (patientId != appointmentDto.PatientId || treatmentId != appointmentDto.TreatmentId)
        {
            return BadRequest("Route ids do not match patient id and treatment id in the request body.");
        }

        var updated = await _appointmentService.UpdateAsync(patientId, treatmentId, appointmentDto, cancellationToken);
        if (updated is null)
        {
            return NotFound($"Appointment for patient {patientId} and treatment {treatmentId} was not found.");
        }

        return Ok(updated);
    }

    [HttpDelete("{patientId:int}/{treatmentId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(
        [FromRoute] int patientId,
        [FromRoute] int treatmentId,
        CancellationToken cancellationToken)
    {
        var deleted = await _appointmentService.DeleteAsync(patientId, treatmentId, cancellationToken);
        if (!deleted)
        {
            return NotFound($"Appointment for patient {patientId} and treatment {treatmentId} was not found.");
        }

        return Ok(new { message = $"Appointment for patient {patientId} and treatment {treatmentId} was removed successfully." });
    }
}
