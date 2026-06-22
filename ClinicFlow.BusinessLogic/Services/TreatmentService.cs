using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.BusinessLogic.Exceptions;
using ClinicFlow.DataAccess.Entities;
using ClinicFlow.DataAccess.Repositories;
using AutoMapper;

namespace ClinicFlow.BusinessLogic.Services;

public class TreatmentService : ITreatmentService
{
    private readonly ITreatmentRepository _treatmentRepository;
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMapper _mapper;

    public TreatmentService(
        ITreatmentRepository treatmentRepository,
        IDoctorRepository doctorRepository,
        IMapper mapper)
    {
        _treatmentRepository = treatmentRepository;
        _doctorRepository = doctorRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<TreatmentDto>> GetAllAsync(int? doctorId = null, CancellationToken cancellationToken = default)
    {
        var treatments = await _treatmentRepository.GetAllAsync(doctorId, cancellationToken);
        return _mapper.Map<IReadOnlyList<TreatmentDto>>(treatments);
    }

    public async Task<TreatmentDto?> GetByIdAsync(int treatmentId, CancellationToken cancellationToken = default)
    {
        var treatment = await _treatmentRepository.GetByIdAsync(treatmentId, cancellationToken);
        return treatment is null ? null : _mapper.Map<TreatmentDto>(treatment);
    }

    public async Task<TreatmentDto> CreateAsync(TreatmentCreateDto createDto, CancellationToken cancellationToken = default)
    {
        await EnsureDoctorExistsAsync(createDto.DoctorId, cancellationToken);

        var treatment = _mapper.Map<Treatment>(createDto);
        var created = await _treatmentRepository.AddAsync(treatment, cancellationToken);
        return _mapper.Map<TreatmentDto>(created);
    }

    public async Task<TreatmentDto?> UpdateAsync(int treatmentId, TreatmentDto treatmentDto, CancellationToken cancellationToken = default)
    {
        var existing = await _treatmentRepository.GetByIdAsync(treatmentId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        await EnsureDoctorExistsAsync(treatmentDto.DoctorId, cancellationToken);

        _mapper.Map(treatmentDto, existing);
        existing.TreatmentId = treatmentId;

        await _treatmentRepository.UpdateAsync(existing, cancellationToken);
        return _mapper.Map<TreatmentDto>(existing);
    }

    public async Task<bool> DeleteAsync(int treatmentId, CancellationToken cancellationToken = default)
    {
        return await _treatmentRepository.DeleteAsync(treatmentId, cancellationToken);
    }

    private async Task EnsureDoctorExistsAsync(int doctorId, CancellationToken cancellationToken)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);
        if (doctor is null)
        {
            throw new NotFoundException($"Doctor with id {doctorId} was not found.");
        }
    }
}
