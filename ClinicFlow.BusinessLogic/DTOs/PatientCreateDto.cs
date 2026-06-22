using System.ComponentModel.DataAnnotations;

namespace ClinicFlow.BusinessLogic.DTOs;

public class PatientCreateDto
{
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;
}
