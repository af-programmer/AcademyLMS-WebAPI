using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.DataAccess.Entities;
using ClinicFlow.DataAccess.Repositories;
using AutoMapper;

namespace ClinicFlow.BusinessLogic.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;
    private readonly IMapper _mapper;

    public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
    {
        _doctorRepository = doctorRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DoctorDto>> GetAllAsync(string? specialty = null, CancellationToken cancellationToken = default)
    {
        var doctors = await _doctorRepository.GetAllAsync(specialty, cancellationToken);
        return _mapper.Map<IReadOnlyList<DoctorDto>>(doctors);
    }

    public async Task<DoctorDto?> GetByIdAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        var doctor = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);
        return doctor is null ? null : _mapper.Map<DoctorDto>(doctor);
    }

    public async Task<DoctorDto> CreateAsync(DoctorCreateDto createDto, CancellationToken cancellationToken = default)
    {
        var doctor = _mapper.Map<Doctor>(createDto);
        var created = await _doctorRepository.AddAsync(doctor, cancellationToken);
        return _mapper.Map<DoctorDto>(created);
    }

    public async Task<DoctorDto?> UpdateAsync(int doctorId, DoctorDto doctorDto, CancellationToken cancellationToken = default)
    {
        var existing = await _doctorRepository.GetByIdAsync(doctorId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(doctorDto, existing);
        existing.DoctorId = doctorId;

        await _doctorRepository.UpdateAsync(existing, cancellationToken);
        return _mapper.Map<DoctorDto>(existing);
    }

    public async Task<bool> DeleteAsync(int doctorId, CancellationToken cancellationToken = default)
    {
        return await _doctorRepository.DeleteAsync(doctorId, cancellationToken);
    }
}
