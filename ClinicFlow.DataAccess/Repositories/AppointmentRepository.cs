using ClinicFlow.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.DataAccess.Repositories;

public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(ClinicFlowDbContext context) : base(context)
    {
    }

    public async Task<IReadOnlyList<Appointment>> GetAllAsync(int? patientId = null, int? treatmentId = null, CancellationToken cancellationToken = default)
    {
        var query = DbSet.AsNoTracking();

        if (patientId.HasValue)
        {
            query = query.Where(a => a.PatientId == patientId.Value);
        }

        if (treatmentId.HasValue)
        {
            query = query.Where(a => a.TreatmentId == treatmentId.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Appointment?> GetByIdAsync(int patientId, int treatmentId, CancellationToken cancellationToken = default)
    {
        return await DbSet.AsNoTracking()
            .FirstOrDefaultAsync(
                a => a.PatientId == patientId && a.TreatmentId == treatmentId,
                cancellationToken);
    }

    public async Task<bool> DeleteAsync(int patientId, int treatmentId, CancellationToken cancellationToken = default)
    {
        var appointment = await DbSet.FindAsync([patientId, treatmentId], cancellationToken);
        if (appointment is null)
        {
            return false;
        }

        DbSet.Remove(appointment);
        await Context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
