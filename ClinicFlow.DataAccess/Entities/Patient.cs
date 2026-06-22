using System.ComponentModel.DataAnnotations;

namespace ClinicFlow.DataAccess.Entities;

public class Patient
{
    [Key]
    public int PatientId { get; set; }

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

    public ICollection<Appointment> Appointments { get; set; } = [];
}
