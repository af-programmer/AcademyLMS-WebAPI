using AcademyLMS.DataAccess.Entities;

namespace AcademyLMS.DataAccess.Repositories;

public interface IEnrollmentRepository
{
    Task<IReadOnlyList<Enrollment>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Enrollment?> GetByIdAsync(int studentId, int courseId, CancellationToken cancellationToken = default);

    Task<Enrollment> AddAsync(Enrollment enrollment, CancellationToken cancellationToken = default);

    Task UpdateAsync(Enrollment enrollment, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int studentId, int courseId, CancellationToken cancellationToken = default);
}
