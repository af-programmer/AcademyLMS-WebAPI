using ClinicFlow.DataAccess.Entities;

namespace ClinicFlow.DataAccess.Repositories;

public interface IAppointmentRepository
{
    Task<IReadOnlyList<Appointment>> GetAllAsync(int? patientId = null, int? treatmentId = null, CancellationToken cancellationToken = default);

    Task<Appointment?> GetByIdAsync(int patientId, int treatmentId, CancellationToken cancellationToken = default);

    Task<Appointment> AddAsync(Appointment appointment, CancellationToken cancellationToken = default);

    Task UpdateAsync(Appointment appointment, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int patientId, int treatmentId, CancellationToken cancellationToken = default);
}
