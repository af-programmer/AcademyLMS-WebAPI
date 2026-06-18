using AcademyLMS.BusinessLogic.Mappings;
using AcademyLMS.BusinessLogic.Services;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AcademyLMS.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(MappingProfile));
        services.AddScoped<IStudentService, StudentService>();
        return services;
    }
}
