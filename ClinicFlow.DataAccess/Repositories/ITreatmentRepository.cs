using ClinicFlow.DataAccess.Entities;

namespace ClinicFlow.DataAccess.Repositories;

public interface ITreatmentRepository
{
    Task<IReadOnlyList<Treatment>> GetAllAsync(int? doctorId = null, CancellationToken cancellationToken = default);

    Task<Treatment?> GetByIdAsync(int treatmentId, CancellationToken cancellationToken = default);

    Task<Treatment> AddAsync(Treatment treatment, CancellationToken cancellationToken = default);

    Task UpdateAsync(Treatment treatment, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int treatmentId, CancellationToken cancellationToken = default);
}
