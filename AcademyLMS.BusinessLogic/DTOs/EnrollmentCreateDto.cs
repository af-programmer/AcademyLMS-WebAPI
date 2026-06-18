using System.ComponentModel.DataAnnotations;

namespace AcademyLMS.BusinessLogic.DTOs;

public class EnrollmentCreateDto
{
    [Required(ErrorMessage = "Student id is required.")]
    public int StudentId { get; set; }

    [Required(ErrorMessage = "Course id is required.")]
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Grade is required.")]
    [Range(0, 100, ErrorMessage = "Grade must be between 0 and 100.")]
    public int Grade { get; set; }

    [Required(ErrorMessage = "Enrollment date is required.")]
    public DateTime EnrollmentDate { get; set; }
}
