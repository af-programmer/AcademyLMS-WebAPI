using AcademyLMS.DataAccess.Entities;

namespace AcademyLMS.DataAccess.Repositories;

public interface ICourseRepository
{
    Task<IReadOnlyList<Course>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Course?> GetByIdAsync(int courseId, CancellationToken cancellationToken = default);

    Task<Course> AddAsync(Course course, CancellationToken cancellationToken = default);

    Task UpdateAsync(Course course, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int courseId, CancellationToken cancellationToken = default);
}
