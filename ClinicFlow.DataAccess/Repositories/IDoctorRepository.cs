using ClinicFlow.DataAccess.Entities;

namespace ClinicFlow.DataAccess.Repositories;

public interface IDoctorRepository : IGenericRepository<Doctor>
{
    Task<IReadOnlyList<Doctor>> GetAllAsync(string? specialty = null, CancellationToken cancellationToken = default);
}
