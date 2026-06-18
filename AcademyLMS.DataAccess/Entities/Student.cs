using System.ComponentModel.DataAnnotations;

namespace AcademyLMS.DataAccess.Entities;

public class Student
{
    [Key]
    public int StudentId { get; set; }

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

    public ICollection<Enrollment> Enrollments { get; set; } = [];
}
