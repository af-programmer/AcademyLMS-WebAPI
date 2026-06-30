using ClinicFlow.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ClinicFlow.DataAccess;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccess(
        this IServiceCollection services,
        string baseConnectionString,
        string contentRootPath,
        IConfiguration configuration)
    {
        var dataDirectory = configuration["Database:DataDirectory"] ?? "DB";
        var databaseFileName = configuration["Database:DatabaseFileName"] ?? "ClinicFlow.mdf";

        var connectionString = DatabaseConnectionBuilder.BuildAttachDbConnectionString(
            baseConnectionString,
            contentRootPath,
            dataDirectory,
            databaseFileName);

        services.AddDbContext<ClinicFlowDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IPatientRepository, PatientRepository>();
        services.AddScoped<ITreatmentRepository, TreatmentRepository>();
        services.AddScoped<IDoctorRepository, DoctorRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();

        return services;
    }
}
