using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.DataAccess.Entities;
using ClinicFlow.DataAccess.Repositories;
using AutoMapper;

namespace ClinicFlow.BusinessLogic.Services;

public class TreatmentService : GenericService<Treatment, TreatmentDto, TreatmentCreateDto>, ITreatmentService
{
    private readonly ITreatmentRepository _treatmentRepository;

    public TreatmentService(ITreatmentRepository treatmentRepository, IMapper mapper)
        : base(treatmentRepository, mapper, (treatment, id) => treatment.TreatmentId = id)
    {
        _treatmentRepository = treatmentRepository;
    }

    public async Task<IReadOnlyList<TreatmentDto>> GetAllAsync(int? doctorId = null, CancellationToken cancellationToken = default)
    {
        var treatments = await _treatmentRepository.GetAllAsync(doctorId, cancellationToken);
        return Mapper.Map<IReadOnlyList<TreatmentDto>>(treatments);
    }
}
