using ClinicFlow.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.DataAccess.Repositories;

public class TreatmentRepository : GenericRepository<Treatment>, ITreatmentRepository
{
    public TreatmentRepository(ClinicFlowDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Treatment>> GetAllAsync(int? doctorId = null, CancellationToken cancellationToken = default)
    {
        if (!doctorId.HasValue)
        {
            return await base.GetAllAsync(cancellationToken);
        }

        return await DbSet.AsNoTracking()
            .Where(t => t.DoctorId == doctorId.Value)
            .ToListAsync(cancellationToken);
    }
}
