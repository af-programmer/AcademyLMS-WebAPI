using AcademyLMS.BusinessLogic.Mappings;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace AcademyLMS.BusinessLogic;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLogic(this IServiceCollection services)
    {
        services.AddAutoMapper(_ => { }, typeof(MappingProfile));
        return services;
    }
}
