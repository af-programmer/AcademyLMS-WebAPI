using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TreatmentsController : ControllerBase
{
    private readonly ITreatmentService _treatmentService;

    public TreatmentsController(ITreatmentService treatmentService)
    {
        _treatmentService = treatmentService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TreatmentDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TreatmentDto>>> GetAll(
        [FromQuery] int? doctorId,
        CancellationToken cancellationToken)
    {
        var treatments = await _treatmentService.GetAllAsync(doctorId, cancellationToken);
        return Ok(treatments);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TreatmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TreatmentDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var treatment = await _treatmentService.GetByIdAsync(id, cancellationToken);
        if (treatment is null)
        {
            return NotFound($"Treatment with id {id} was not found.");
        }

        return Ok(treatment);
    }

    [HttpPost]
    [ProducesResponseType(typeof(TreatmentDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TreatmentDto>> Create(
        [FromBody] TreatmentCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _treatmentService.CreateAsync(createDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.TreatmentId }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TreatmentDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TreatmentDto>> Update(
        [FromRoute] int id,
        [FromBody] TreatmentDto treatmentDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != treatmentDto.TreatmentId)
        {
            return BadRequest("Route id does not match treatment id in the request body.");
        }

        var updated = await _treatmentService.UpdateAsync(id, treatmentDto, cancellationToken);
        if (updated is null)
        {
            return NotFound($"Treatment with id {id} was not found.");
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
        var deleted = await _treatmentService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound($"Treatment with id {id} was not found.");
        }

        return Ok(new { message = $"Treatment with id {id} was deleted successfully." });
    }
}
