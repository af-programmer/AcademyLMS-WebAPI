using AcademyLMS.BusinessLogic.DTOs;

namespace AcademyLMS.BusinessLogic.Services;

public interface IEnrollmentService
{
    Task<IReadOnlyList<EnrollmentDto>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<EnrollmentDto?> GetByIdAsync(int studentId, int courseId, CancellationToken cancellationToken = default);

    Task<EnrollmentDto?> CreateAsync(EnrollmentCreateDto createDto, CancellationToken cancellationToken = default);

    Task<EnrollmentDto?> UpdateAsync(int studentId, int courseId, EnrollmentDto enrollmentDto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int studentId, int courseId, CancellationToken cancellationToken = default);
}
