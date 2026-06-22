using ClinicFlow.BusinessLogic.Mappings;
using ClinicFlow.BusinessLogic.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicFlow.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(MappingProfile));
        services.AddScoped<IPatientService, PatientService>();
        services.AddScoped<ITreatmentService, TreatmentService>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        return services;
    }
}
