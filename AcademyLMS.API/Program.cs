using AcademyLMS.API.Middleware;
using AcademyLMS.BusinessLogic;
using AcademyLMS.DataAccess;
using AcademyLMS.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var baseConnectionString = builder.Configuration.GetConnectionString("AcademyDb")
    ?? throw new InvalidOperationException("Connection string 'AcademyDb' was not found.");

var connectionString = DatabaseConnectionBuilder.BuildAttachDbConnectionString(
    baseConnectionString,
    builder.Environment.ContentRootPath,
    builder.Configuration["Database:DataDirectory"] ?? "DB",
    builder.Configuration["Database:DatabaseFileName"] ?? "AcademyLMS.mdf");

builder.Services.AddDbContext<AcademyDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();

builder.Services.AddBusinessLogic();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandling();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
