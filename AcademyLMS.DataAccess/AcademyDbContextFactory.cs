using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace AcademyLMS.DataAccess;

public class AcademyDbContextFactory : IDesignTimeDbContextFactory<AcademyDbContext>
{
    public AcademyDbContext CreateDbContext(string[] args)
    {
        var apiProjectPath = ResolveApiProjectPath();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(apiProjectPath)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var baseConnectionString = configuration.GetConnectionString("AcademyDb")
            ?? throw new InvalidOperationException("Connection string 'AcademyDb' was not found.");

        var connectionString = DatabaseConnectionBuilder.BuildAttachDbConnectionString(
            baseConnectionString,
            apiProjectPath,
            configuration["Database:DataDirectory"] ?? "DB",
            configuration["Database:DatabaseFileName"] ?? "AcademyLMS.mdf");

        var optionsBuilder = new DbContextOptionsBuilder<AcademyDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new AcademyDbContext(optionsBuilder.Options);
    }

    private static string ResolveApiProjectPath()
    {
        var candidates = new[]
        {
            Path.Combine(Directory.GetCurrentDirectory(), "AcademyLMS.API"),
            Directory.GetCurrentDirectory(),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "AcademyLMS.API")),
            Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "..", "AcademyLMS.API"))
        };

        foreach (var path in candidates.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            if (File.Exists(Path.Combine(path, "appsettings.json")))
            {
                return path;
            }
        }

        throw new InvalidOperationException(
            "Could not locate the AcademyLMS.API project directory for EF Core design-time tooling.");
    }
}
