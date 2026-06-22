using ClinicFlow.BusinessLogic.DTOs;

namespace ClinicFlow.BusinessLogic.Services;

public interface ITreatmentService
{
    Task<IReadOnlyList<TreatmentDto>> GetAllAsync(int? doctorId = null, CancellationToken cancellationToken = default);

    Task<TreatmentDto?> GetByIdAsync(int treatmentId, CancellationToken cancellationToken = default);

    Task<TreatmentDto> CreateAsync(TreatmentCreateDto createDto, CancellationToken cancellationToken = default);

    Task<TreatmentDto?> UpdateAsync(int treatmentId, TreatmentDto treatmentDto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int treatmentId, CancellationToken cancellationToken = default);
}
