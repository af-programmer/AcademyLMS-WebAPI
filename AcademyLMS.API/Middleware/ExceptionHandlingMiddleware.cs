using AcademyLMS.API.Models;

namespace AcademyLMS.API.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "An unhandled exception occurred while processing {Method} {Path}.",
                context.Request.Method,
                context.Request.Path);

            await HandleExceptionAsync(context);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;

        var response = new ErrorResponse
        {
            StatusCode = StatusCodes.Status500InternalServerError,
            Message = "An unexpected error occurred. Please try again later.",
            TraceId = context.TraceIdentifier
        };

        return context.Response.WriteAsJsonAsync(response);
    }
}
