using ClinicFlow.API.Middleware;
using ClinicFlow.BusinessLogic;
using ClinicFlow.DataAccess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var baseConnectionString = builder.Configuration.GetConnectionString("ClinicFlowDb")
    ?? throw new InvalidOperationException("Connection string 'ClinicFlowDb' was not found.");

var connectionString = DatabaseConnectionBuilder.BuildAttachDbConnectionString(
    baseConnectionString,
    builder.Environment.ContentRootPath,
    builder.Configuration["Database:DataDirectory"] ?? "DB",
    builder.Configuration["Database:DatabaseFileName"] ?? "ClinicFlow.mdf");

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
            Title = "ClinicFlow API",
            Version = "v1",
            Description = "REST API for managing patients, treatments, doctors, and appointments."
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
        swaggerOptions.SwaggerEndpoint("/openapi/v1.json", "ClinicFlow API v1");
        swaggerOptions.DocumentTitle = "ClinicFlow API";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
