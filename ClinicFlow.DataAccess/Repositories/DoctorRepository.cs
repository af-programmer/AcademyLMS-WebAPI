using ClinicFlow.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.DataAccess.Repositories;

public class DoctorRepository : GenericRepository<Doctor>, IDoctorRepository
{
    public DoctorRepository(ClinicFlowDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Doctor>> GetAllAsync(string? specialty = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(specialty))
        {
            return await base.GetAllAsync(cancellationToken);
        }

        var normalizedSpecialty = specialty.Trim().ToLower();
        return await DbSet.AsNoTracking()
            .Where(d => d.Specialty != null && d.Specialty.ToLower() == normalizedSpecialty)
            .ToListAsync(cancellationToken);
    }
}
