using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.DataAccess.Entities;
using ClinicFlow.DataAccess.Repositories;
using AutoMapper;

namespace ClinicFlow.BusinessLogic.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepository;
    private readonly IMapper _mapper;

    public PatientService(IPatientRepository patientRepository, IMapper mapper)
    {
        _patientRepository = patientRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<PatientDto>> GetAllAsync(string? email = null, CancellationToken cancellationToken = default)
    {
        var patients = await _patientRepository.GetAllAsync(email, cancellationToken);
        return _mapper.Map<IReadOnlyList<PatientDto>>(patients);
    }

    public async Task<PatientDto?> GetByIdAsync(int patientId, CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByIdAsync(patientId, cancellationToken);
        return patient is null ? null : _mapper.Map<PatientDto>(patient);
    }

    public async Task<PatientDto> CreateAsync(PatientCreateDto createDto, CancellationToken cancellationToken = default)
    {
        var patient = _mapper.Map<Patient>(createDto);
        var created = await _patientRepository.AddAsync(patient, cancellationToken);
        return _mapper.Map<PatientDto>(created);
    }

    public async Task<PatientDto?> UpdateAsync(int patientId, PatientDto patientDto, CancellationToken cancellationToken = default)
    {
        var existing = await _patientRepository.GetByIdAsync(patientId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(patientDto, existing);
        existing.PatientId = patientId;

        await _patientRepository.UpdateAsync(existing, cancellationToken);
        return _mapper.Map<PatientDto>(existing);
    }

    public async Task<bool> DeleteAsync(int patientId, CancellationToken cancellationToken = default)
    {
        return await _patientRepository.DeleteAsync(patientId, cancellationToken);
    }
}
