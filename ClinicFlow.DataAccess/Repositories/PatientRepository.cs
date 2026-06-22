using ClinicFlow.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicFlow.DataAccess.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ClinicFlowDbContext _context;

    public PatientRepository(ClinicFlowDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyList<Patient>> GetAllAsync(string? email = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Patients.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(email))
        {
            var normalizedEmail = email.Trim().ToLower();
            query = query.Where(p => p.Email.ToLower() == normalizedEmail);
        }

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<Patient?> GetByIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _context.Patients
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.PatientId == patientId, cancellationToken);
    }

    public async Task<Patient> AddAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        await _context.Patients.AddAsync(patient, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return patient;
    }

    public async Task UpdateAsync(Patient patient, CancellationToken cancellationToken = default)
    {
        _context.Patients.Update(patient);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var patient = await _context.Patients.FindAsync([patientId], cancellationToken);
        if (patient is null)
        {
            return false;
        }

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
