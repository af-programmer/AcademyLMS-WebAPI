using ClinicFlow.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.DataAccess.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ClinicFlowDbContext _context;

    public DoctorRepository(ClinicFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Doctor>> GetAllAsync(string? specialty = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Doctors.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(specialty))
        {
            var normalizedSpecialty = specialty.Trim().ToLower();
            query = query.Where(d =>
                d.Specialty != null &&
                d.Specialty.ToLower() == normalizedSpecialty);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Doctor?> GetByIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        return await _context.Doctors
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.DoctorId == doctorId, cancellationToken);
    }

    public async Task<Doctor> AddAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        await _context.Doctors.AddAsync(doctor, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return doctor;
    }

    public async Task UpdateAsync(Doctor doctor, CancellationToken cancellationToken = default)
    {
        _context.Doctors.Update(doctor);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        var doctor = await _context.Doctors.FindAsync([doctorId], cancellationToken);
        if (doctor is null)
        {
            return false;
        }

        _context.Doctors.Remove(doctor);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
