using AcademyLMS.BusinessLogic.DTOs;

namespace AcademyLMS.BusinessLogic.Services;

public interface ITeacherService
{
    Task<IReadOnlyList<TeacherDto>> GetAllAsync(string? department = null, CancellationToken cancellationToken = default);

    Task<TeacherDto?> GetByIdAsync(int teacherId, CancellationToken cancellationToken = default);

    Task<TeacherDto> CreateAsync(TeacherCreateDto createDto, CancellationToken cancellationToken = default);

    Task<TeacherDto?> UpdateAsync(int teacherId, TeacherDto teacherDto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int teacherId, CancellationToken cancellationToken = default);
}
