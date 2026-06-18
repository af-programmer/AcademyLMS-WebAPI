using AcademyLMS.DataAccess.Entities;

namespace AcademyLMS.DataAccess.Repositories;

public interface ITeacherRepository
{
    Task<IReadOnlyList<Teacher>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<Teacher?> GetByIdAsync(int teacherId, CancellationToken cancellationToken = default);

    Task<Teacher> AddAsync(Teacher teacher, CancellationToken cancellationToken = default);

    Task UpdateAsync(Teacher teacher, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int teacherId, CancellationToken cancellationToken = default);
}
