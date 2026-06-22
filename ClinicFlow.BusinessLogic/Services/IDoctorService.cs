using ClinicFlow.BusinessLogic.DTOs;

namespace ClinicFlow.BusinessLogic.Services;

public interface IDoctorService
{
    Task<IReadOnlyList<DoctorDto>> GetAllAsync(string? specialty = null, CancellationToken cancellationToken = default);

    Task<DoctorDto?> GetByIdAsync(int doctorId, CancellationToken cancellationToken = default);

    Task<DoctorDto> CreateAsync(DoctorCreateDto createDto, CancellationToken cancellationToken = default);

    Task<DoctorDto?> UpdateAsync(int doctorId, DoctorDto doctorDto, CancellationToken cancellationToken = default);

    Task<bool> DeleteAsync(int doctorId, CancellationToken cancellationToken = default);
}
