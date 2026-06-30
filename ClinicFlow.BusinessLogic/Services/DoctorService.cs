using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.DataAccess.Entities;
using ClinicFlow.DataAccess.Repositories;
using AutoMapper;

namespace ClinicFlow.BusinessLogic.Services;

public class DoctorService : GenericService<Doctor, DoctorDto, DoctorCreateDto>, IDoctorService
{
    private readonly IDoctorRepository _doctorRepository;

    public DoctorService(IDoctorRepository doctorRepository, IMapper mapper)
        : base(doctorRepository, mapper, (doctor, id) => doctor.DoctorId = id)
    {
        _doctorRepository = doctorRepository;
    }

    public async Task<IReadOnlyList<DoctorDto>> GetAllAsync(string? specialty = null, CancellationToken cancellationToken = default)
    {
        var doctors = await _doctorRepository.GetAllAsync(specialty, cancellationToken);
        return Mapper.Map<IReadOnlyList<DoctorDto>>(doctors);
    }
}
