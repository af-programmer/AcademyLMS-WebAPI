using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.DataAccess.Entities;
using ClinicFlow.DataAccess.Repositories;
using AutoMapper;

namespace ClinicFlow.BusinessLogic.Services;

public class PatientService : GenericService<Patient, PatientDto, PatientCreateDto>, IPatientService
{
    private readonly IPatientRepository _patientRepository;

    public PatientService(IPatientRepository patientRepository, IMapper mapper)
        : base(patientRepository, mapper, (patient, id) => patient.PatientId = id)
    {
        _patientRepository = patientRepository;
    }

    public async Task<IReadOnlyList<PatientDto>> GetAllAsync(string? email = null, CancellationToken cancellationToken = default)
    {
        var patients = await _patientRepository.GetAllAsync(email, cancellationToken);
        return Mapper.Map<IReadOnlyList<PatientDto>>(patients);
    }
}
