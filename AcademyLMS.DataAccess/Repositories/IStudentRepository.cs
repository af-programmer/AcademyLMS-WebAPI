using AcademyLMS.DataAccess.Entities;

namespace AcademyLMS.DataAccess.Repositories;

public interface IStudentRepository
{
    Task<IReadOnlyList<Student>> GetAllAsync(string? email = null, CancellationToken cancellationToken = default);

    Task<Student?> GetByIdAsync(int studentId, CancellationToken cancellationToken = default);

    Task<Student> AddAsync(Student student, CancellationToken cancellationToken = default);

    Task UpdateAsync(Student student, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int studentId, CancellationToken cancellationToken = default);
}
