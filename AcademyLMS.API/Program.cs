using AcademyLMS.BusinessLogic;
using AcademyLMS.DataAccess;
using AcademyLMS.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<AcademyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AcademyDb")));

builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddBusinessLogic();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
