using AcademyLMS.API.Middleware;
using AcademyLMS.BusinessLogic;
using AcademyLMS.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var baseConnectionString = builder.Configuration.GetConnectionString("AcademyDb")
    ?? throw new InvalidOperationException("Connection string 'AcademyDb' was not found.");

var connectionString = DatabaseConnectionBuilder.BuildAttachDbConnectionString(
    baseConnectionString,
    builder.Environment.ContentRootPath,
    builder.Configuration["Database:DataDirectory"] ?? "DB",
    builder.Configuration["Database:DatabaseFileName"] ?? "AcademyLMS.mdf");

builder.Services.AddDataAccess(connectionString);
builder.Services.AddBusinessLogic();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info = new()
        {
            Title = "Academy LMS API",
            Version = "v1",
            Description = "REST API for managing students, courses, teachers, and enrollments in the Academy LMS."
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.UseExceptionHandling();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(swaggerOptions =>
    {
        swaggerOptions.SwaggerEndpoint("/openapi/v1.json", "Academy LMS API v1");
        swaggerOptions.DocumentTitle = "Academy LMS API";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
