namespace ClinicFlow.DataAccess;

public static class DatabaseConnectionBuilder
{
    public static string BuildAttachDbConnectionString(
        string baseConnectionString,
        string contentRootPath,
        string dataDirectory = "DB",
        string databaseFileName = "ClinicFlow.mdf")
    {
        var dbDirectory = Path.GetFullPath(Path.Combine(contentRootPath, "..", dataDirectory));
        Directory.CreateDirectory(dbDirectory);

        var dbFilePath = Path.Combine(dbDirectory, databaseFileName);

        return $"{baseConnectionString.TrimEnd(';')};AttachDbFilename={dbFilePath}";
    }
}
