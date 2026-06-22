using System.ComponentModel.DataAnnotations;

namespace ClinicFlow.DataAccess.Entities;

public class Doctor
{
    [Key]
    public int DoctorId { get; set; }

    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Specialty { get; set; }

    public ICollection<Treatment> Treatments { get; set; } = [];
}
