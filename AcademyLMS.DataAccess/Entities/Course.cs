using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademyLMS.DataAccess.Entities;

public class Course
{
    [Key]
    public int CourseId { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required]
    public int Credits { get; set; }

    [Required]
    public int TeacherId { get; set; }

    [ForeignKey(nameof(TeacherId))]
    public Teacher Teacher { get; set; } = null!;

    public ICollection<Enrollment> Enrollments { get; set; } = [];
}
