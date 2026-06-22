using ClinicFlow.DataAccess.Entities;

namespace ClinicFlow.DataAccess.Repositories;

public interface IPatientRepository
{
    Task<IReadOnlyList<Patient>> GetAllAsync(string? email = null, CancellationToken cancellationToken = default);

    Task<Patient?> GetByIdAsync(int patientId, CancellationToken cancellationToken = default);

    Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default);

    Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int patientId, CancellationToken cancellationToken = default);
}
