using ClinicFlow.DataAccess.Entities;

namespace ClinicFlow.DataAccess.Repositories;

public interface ITreatmentRepository : IGenericRepository<Treatment>
{
    Task<IReadOnlyList<Treatment>> GetAllAsync(int? doctorId = null, CancellationToken cancellationToken = default);
}
