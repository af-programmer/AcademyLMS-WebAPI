using ClinicFlow.BusinessLogic.DTOs;

namespace ClinicFlow.BusinessLogic.Services;

public interface IPatientService
{
    Task<IReadOnlyList<PatientDto>> GetAllAsync(string? email = null, CancellationToken cancellationToken = default);

    Task<PatientDto?> GetByIdAsync(int patientId, CancellationToken cancellationToken = default);

    Task<PatientDto> CreateAsync(PatientCreateDto createDto, CancellationToken cancellationToken = default);

    Task<PatientDto?> UpdateAsync(int patientId, PatientDto patientDto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int patientId, CancellationToken cancellationToken = default);
}
