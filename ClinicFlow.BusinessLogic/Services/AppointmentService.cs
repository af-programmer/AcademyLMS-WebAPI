using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.BusinessLogic.Exceptions;
using ClinicFlow.DataAccess.Entities;
using ClinicFlow.DataAccess.Repositories;
using AutoMapper;

namespace ClinicFlow.BusinessLogic.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IPatientRepository _patientRepository;
    private readonly ITreatmentRepository _treatmentRepository;
    private readonly IMapper _mapper;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IPatientRepository patientRepository,
        ITreatmentRepository treatmentRepository,
        IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _patientRepository = patientRepository;
        _treatmentRepository = treatmentRepository;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<AppointmentDto>> GetAllAsync(int? patientId = null, int? treatmentId = null, CancellationToken cancellationToken = default)
    {
        var appointments = await _appointmentRepository.GetAllAsync(patientId, treatmentId, cancellationToken);
        return _mapper.Map<IReadOnlyList<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto?> GetByIdAsync(int patientId, int treatmentId, CancellationToken cancellationToken = default)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(patientId, treatmentId, cancellationToken);
        return appointment is null ? null : _mapper.Map<AppointmentDto>(appointment);
    }

    public async Task<AppointmentDto> CreateAsync(AppointmentCreateDto createDto, CancellationToken cancellationToken = default)
    {
        var patient = await _patientRepository.GetByIdAsync(createDto.PatientId, cancellationToken);
        if (patient is null)
        {
            throw new NotFoundException($"Patient with id {createDto.PatientId} was not found.");
        }

        var treatment = await _treatmentRepository.GetByIdAsync(createDto.TreatmentId, cancellationToken);
        if (treatment is null)
        {
            throw new NotFoundException($"Treatment with id {createDto.TreatmentId} was not found.");
        }

        var existing = await _appointmentRepository.GetByIdAsync(createDto.PatientId, createDto.TreatmentId, cancellationToken);
        if (existing is not null)
        {
            throw new ConflictException(
                $"Patient {createDto.PatientId} already has an appointment for treatment {createDto.TreatmentId}.");
        }

        var appointment = _mapper.Map<Appointment>(createDto);
        var created = await _appointmentRepository.AddAsync(appointment, cancellationToken);
        return _mapper.Map<AppointmentDto>(created);
    }

    public async Task<AppointmentDto?> UpdateAsync(
        int patientId,
        int treatmentId,
        AppointmentDto appointmentDto,
        CancellationToken cancellationToken = default)
    {
        var existing = await _appointmentRepository.GetByIdAsync(patientId, treatmentId, cancellationToken);
        if (existing is null)
        {
            return null;
        }

        _mapper.Map(appointmentDto, existing);
        existing.PatientId = patientId;
        existing.TreatmentId = treatmentId;

        await _appointmentRepository.UpdateAsync(existing, cancellationToken);
        return _mapper.Map<AppointmentDto>(existing);
    }

    public async Task<bool> DeleteAsync(int patientId, int treatmentId, CancellationToken cancellationToken = default)
    {
        return await _appointmentRepository.DeleteAsync(patientId, treatmentId, cancellationToken);
    }
}
