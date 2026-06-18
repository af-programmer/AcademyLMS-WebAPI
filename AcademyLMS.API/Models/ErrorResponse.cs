namespace AcademyLMS.API.Models;

public class ErrorResponse
{
    public int StatusCode { get; set; }

    public string Message { get; set; } = string.Empty;

    public string TraceId { get; set; } = string.Empty;
}
