namespace AcademyLMS.BusinessLogic.DTOs;

public class CourseDto
{
    public int CourseId { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public int Credits { get; set; }

    public int TeacherId { get; set; }
}
