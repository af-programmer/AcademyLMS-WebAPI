using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClinicFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<DoctorDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<DoctorDto>>> GetAll(
        [FromQuery] string? specialty,
        CancellationToken cancellationToken)
    {
        var doctors = await _doctorService.GetAllAsync(specialty, cancellationToken);
        return Ok(doctors);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DoctorDto>> GetById(
        [FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var doctor = await _doctorService.GetByIdAsync(id, cancellationToken);
        if (doctor is null)
        {
            return NotFound($"Doctor with id {id} was not found.");
        }

        return Ok(doctor);
    }

    [HttpPost]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DoctorDto>> Create(
        [FromBody] DoctorCreateDto createDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var created = await _doctorService.CreateAsync(createDto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.DoctorId }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(DoctorDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DoctorDto>> Update(
        [FromRoute] int id,
        [FromBody] DoctorDto doctorDto,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (id != doctorDto.DoctorId)
        {
            return BadRequest("Route id does not match doctor id in the request body.");
        }

        var updated = await _doctorService.UpdateAsync(id, doctorDto, cancellationToken);
        if (updated is null)
        {
            return NotFound($"Doctor with id {id} was not found.");
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
        var deleted = await _doctorService.DeleteAsync(id, cancellationToken);
        if (!deleted)
        {
            return NotFound($"Doctor with id {id} was not found.");
        }

        return Ok(new { message = $"Doctor with id {id} was deleted successfully." });
    }
}
