using System.ComponentModel.DataAnnotations;

namespace ClinicFlow.BusinessLogic.DTOs;

public class TreatmentCreateDto
{
    [Required(ErrorMessage = "Name is required.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Duration is required.")]
    [Range(5, 480, ErrorMessage = "Duration must be between 5 and 480 minutes.")]
    public int DurationMinutes { get; set; }

    [Required(ErrorMessage = "Doctor id is required.")]
    public int DoctorId { get; set; }
}
