using ClinicFlow.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.DataAccess.Repositories;

public class PatientRepository : GenericRepository<Patient>, IPatientRepository
{
    public PatientRepository(ClinicFlowDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Patient>> GetAllAsync(string? email = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return await base.GetAllAsync(cancellationToken);
        }

        var normalizedEmail = email.Trim().ToLower();
        return await DbSet.AsNoTracking()
            .Where(p => p.Email.ToLower() == normalizedEmail)
            .ToListAsync(cancellationToken);
    }
}
