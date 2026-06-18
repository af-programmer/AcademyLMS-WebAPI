using System.ComponentModel.DataAnnotations;

namespace AcademyLMS.BusinessLogic.DTOs;

public class CourseDto
{
    [Required]
    public int CourseId { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Credits is required.")]
    [Range(1, 30, ErrorMessage = "Credits must be between 1 and 30.")]
    public int Credits { get; set; }

    [Required(ErrorMessage = "Teacher id is required.")]
    public int TeacherId { get; set; }
}
