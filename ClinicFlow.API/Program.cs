using ClinicFlow.API.Middleware;
using ClinicFlow.BusinessLogic;
using ClinicFlow.DataAccess;

var builder = WebApplication.CreateBuilder(args);

var baseConnectionString = builder.Configuration.GetConnectionString("ClinicFlowDb")
    ?? throw new InvalidOperationException("Connection string 'ClinicFlowDb' was not found.");

builder.Services.AddDataAccess(baseConnectionString, builder.Environment.ContentRootPath, builder.Configuration);
builder.Services.AddBusinessLogic();

builder.Services.AddControllers();
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

app.MapControllers();

app.Run();
