using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ClinicFlow.DataAccess;

public class ClinicFlowDbContextFactory : IDesignTimeDbContextFactory<ClinicFlowDbContext>
{
    public ClinicFlowDbContext CreateDbContext(string[] args)
    {
        var apiProjectPath = ResolveApiProjectPath();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var baseConnectionString = configuration.GetConnectionString("ClinicFlowDb")
            ?? throw new InvalidOperationException("Connection string 'ClinicFlowDb' was not found.");

        var connectionString = DatabaseConnectionBuilder.BuildAttachDbConnectionString(
            baseConnectionString,
            apiProjectPath,
            configuration["Database:DataDirectory"] ?? "DB",
            configuration["Database:DatabaseFileName"] ?? "ClinicFlow.mdf");

        var optionsBuilder = new DbContextOptionsBuilder<ClinicFlowDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ClinicFlowDbContext(optionsBuilder.Options);
    }

    private static string ResolveApiProjectPath()
    {
        var candidates = new[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), "ClinicFlow.API"),
            Directory.GetCurrentDirectory(),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "ClinicFlow.API")),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "ClinicFlow.API"))
        };

        foreach (var path in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (File.Exists(Path.Combine(path, "appsettings.json")))
            {
                return path;
            }
        }

        throw new InvalidOperationException(
            "Could not locate the ClinicFlow.API project directory for EF Core design-time tooling.");
    }
}
