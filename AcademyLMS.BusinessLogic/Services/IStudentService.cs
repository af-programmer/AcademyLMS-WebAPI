using AcademyLMS.BusinessLogic.DTOs;

namespace AcademyLMS.BusinessLogic.Services;

public interface IStudentService
{
    Task<IReadOnlyList<StudentDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<StudentDto?> GetByIdAsync(int studentId, CancellationToken cancellationToken = default);

    Task<StudentDto> CreateAsync(StudentCreateDto createDto, CancellationToken cancellationToken = default);

    Task<StudentDto?> UpdateAsync(int studentId, StudentDto studentDto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int studentId, CancellationToken cancellationToken = default);
}
