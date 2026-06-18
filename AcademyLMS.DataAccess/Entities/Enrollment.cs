using System.ComponentModel.DataAnnotations;

namespace AcademyLMS.DataAccess.Entities;

public class Enrollment
{
    public int StudentId { get; set; }

    public int CourseId { get; set; }

    [Required]
    public int Grade { get; set; }

    [Required]
    public DateTime EnrollmentDate { get; set; }

    public Student Student { get; set; } = null!;

    public Course Course { get; set; } = null!;
}
