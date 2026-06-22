using ClinicFlow.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.DataAccess.Repositories;

public class TreatmentRepository : ITreatmentRepository
{
    private readonly ClinicFlowDbContext _context;

    public TreatmentRepository(ClinicFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Treatment>> GetAllAsync(int? doctorId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Treatments.AsNoTracking();

        if (doctorId.HasValue)
        {
            query = query.Where(t => t.DoctorId == doctorId.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Treatment?> GetByIdAsync(int treatmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Treatments
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TreatmentId == treatmentId, cancellationToken);
    }

    public async Task<Treatment> AddAsync(Treatment treatment, CancellationToken cancellationToken = default)
    {
        await _context.Treatments.AddAsync(treatment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return treatment;
    }

    public async Task UpdateAsync(Treatment treatment, CancellationToken cancellationToken = default)
    {
        _context.Treatments.Update(treatment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int treatmentId, CancellationToken cancellationToken = default)
    {
        var treatment = await _context.Treatments.FindAsync([treatmentId], cancellationToken);
        if (treatment is null)
        {
            return false;
        }

        _context.Treatments.Remove(treatment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
