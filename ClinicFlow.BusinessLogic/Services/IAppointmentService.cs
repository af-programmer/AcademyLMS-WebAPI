using ClinicFlow.BusinessLogic.DTOs;

namespace ClinicFlow.BusinessLogic.Services;

public interface IAppointmentService
{
    Task<IReadOnlyList<AppointmentDto>> GetAllAsync(int? patientId = null, int? treatmentId = null, CancellationToken cancellationToken = default);

    Task<AppointmentDto?> GetByIdAsync(int patientId, int treatmentId, CancellationToken cancellationToken = default);

    Task<AppointmentDto> CreateAsync(AppointmentCreateDto createDto, CancellationToken cancellationToken = default);

    Task<AppointmentDto?> UpdateAsync(
        int patientId,
        int treatmentId,
        AppointmentDto appointmentDto,
        CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int patientId, int treatmentId, CancellationToken cancellationToken = default);
}
