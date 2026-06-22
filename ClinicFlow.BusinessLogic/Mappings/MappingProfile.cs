using ClinicFlow.BusinessLogic.DTOs;
using ClinicFlow.DataAccess.Entities;
using AutoMapper;

namespace ClinicFlow.BusinessLogic.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Patient, PatientDto>();

        CreateMap<PatientDto, Patient>()
            .ForMember(dest => dest.Appointments, opt => opt.Ignore());

        CreateMap<PatientCreateDto, Patient>()
            .ForMember(dest => dest.PatientId, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore());

        CreateMap<Patient, PatientCreateDto>();

        CreateMap<Treatment, TreatmentDto>();

        CreateMap<TreatmentDto, Treatment>()
            .ForMember(dest => dest.Doctor, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore());

        CreateMap<TreatmentCreateDto, Treatment>()
            .ForMember(dest => dest.TreatmentId, opt => opt.Ignore())
            .ForMember(dest => dest.Doctor, opt => opt.Ignore())
            .ForMember(dest => dest.Appointments, opt => opt.Ignore());

        CreateMap<Treatment, TreatmentCreateDto>();

        CreateMap<Doctor, DoctorDto>();

        CreateMap<DoctorDto, Doctor>()
            .ForMember(dest => dest.Treatments, opt => opt.Ignore());

        CreateMap<DoctorCreateDto, Doctor>()
            .ForMember(dest => dest.DoctorId, opt => opt.Ignore())
            .ForMember(dest => dest.Treatments, opt => opt.Ignore());

        CreateMap<Doctor, DoctorCreateDto>();

        CreateMap<Appointment, AppointmentDto>();

        CreateMap<AppointmentDto, Appointment>()
            .ForMember(dest => dest.Patient, opt => opt.Ignore())
            .ForMember(dest => dest.Treatment, opt => opt.Ignore());

        CreateMap<AppointmentCreateDto, Appointment>()
            .ForMember(dest => dest.Patient, opt => opt.Ignore())
            .ForMember(dest => dest.Treatment, opt => opt.Ignore());

        CreateMap<Appointment, AppointmentCreateDto>();
    }
}
