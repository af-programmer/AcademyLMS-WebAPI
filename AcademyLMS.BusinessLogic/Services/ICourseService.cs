using AcademyLMS.BusinessLogic.DTOs;

namespace AcademyLMS.BusinessLogic.Services;

public interface ICourseService
{
    Task<IReadOnlyList<CourseDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<CourseDto?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default);

    Task<CourseDto> CreateAsync(CourseCreateDto createDto, CancellationToken cancellationToken = default);

    Task<CourseDto?> UpdateAsync(int courseId, CourseDto courseDto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int courseId, CancellationToken cancellationToken = default);
}
