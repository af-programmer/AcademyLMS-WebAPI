using ClinicFlow.DataAccess.Entities;

namespace ClinicFlow.DataAccess.Repositories;

public interface IPatientRepository : IGenericRepository<Patient>
{
    Task<IReadOnlyList<Patient>> GetAllAsync(string? email = null, CancellationToken cancellationToken = default);
}
