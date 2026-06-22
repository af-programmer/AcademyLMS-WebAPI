using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PatientDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PatientDto>>> GetAll(
        [FromQuery] string? email,
        CancellationToken cancellationToken)
    {
        var patients = await _patientService.GetAllAsync(email, cancellationToken);
        return Ok(patients);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var patient = await _patientService.GetByIdAsync(id, cancellationToken);
        if (patient is null)
        {
            return NotFound($"Patient with id {id} was not found.");
        }

        return Ok(patient);
    }

    [HttpPost]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PatientDto>> Create(
        [FromBody] PatientCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _patientService.CreateAsync(createDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.PatientId }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(PatientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PatientDto>> Update(
        [FromRoute] int id,
        [FromBody] PatientDto patientDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != patientDto.PatientId)
        {
            return BadRequest("Route id does not match patient id in the request body.");
        }

        var updated = await _patientService.UpdateAsync(id, patientDto, cancellationToken);
        if (updated is null)
        {
            return NotFound($"Patient with id {id} was not found.");
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
        var deleted = await _patientService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound($"Patient with id {id} was not found.");
        }

        return Ok(new { message = $"Patient with id {id} was deleted successfully." });
    }
}
