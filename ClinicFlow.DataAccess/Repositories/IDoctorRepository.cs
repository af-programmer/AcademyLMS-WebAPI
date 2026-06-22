using ClinicFlow.DataAccess.Entities;

namespace ClinicFlow.DataAccess.Repositories;

public interface IDoctorRepository
{
    Task<IReadOnlyList<Doctor>> GetAllAsync(string? specialty = null, CancellationToken cancellationToken = default);

    Task<Doctor?> GetByIdAsync(int doctorId, CancellationToken cancellationToken = default);

    Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default);

    Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int doctorId, CancellationToken cancellationToken = default);
}
