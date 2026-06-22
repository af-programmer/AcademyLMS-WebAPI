using ClinicFlow.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.DataAccess.Repositories;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ClinicFlowDbContext _context;

    public AppointmentRepository(ClinicFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Appointment>> GetAllAsync(int? patientId = null, int? treatmentId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Appointments.AsNoTracking();

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
        return await _context.Appointments
            .AsNoTracking()
            .FirstOrDefaultAsync(
                a => a.PatientId == patientId && a.TreatmentId == treatmentId,
                cancellationToken);
    }

    public async Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        await _context.Appointments.AddAsync(appointment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return appointment;
    }

    public async Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        _context.Appointments.Update(appointment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int patientId, int treatmentId, CancellationToken cancellationToken = default)
    {
        var appointment = await _context.Appointments.FindAsync([patientId, treatmentId], cancellationToken);
        if (appointment is null)
        {
            return false;
        }

        _context.Appointments.Remove(appointment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
