using FluentValidation;
using IndividualsRegistry.Application;
using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Infrastructure.Data;
using IndividualsRegistry.Infrastructure.Models.Configuration;
using IndividualsRegistry.Infrastructure.Repositories;
using IndividualsRegistry.Presentation.Api.Configuration;
using IndividualsRegistry.Presentation.Api.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.ConfigureOptions<CultureOptionsSetup>();

var applicationAssembly = typeof(AssemblyReference).Assembly;
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblyContaining<ApplicationMarker>();
builder.Services.AddLocalization(x => x.ResourcesPath = "Resources");

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IIndividualsRepository, IndividualsRepository>();

builder.Services.AddTransient<ErrorLoggingMiddleware>();
builder.Services.AddTransient<CultureReaderMiddleware>();

var connStr = builder.Configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
builder.Services.AddDbContext<IndividualsDbContext>(opt => opt.UseSqlServer(connStr!.MainDb));

builder.Host.UseSerilog(
    (context, services, configuration) =>
        configuration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(services)
            .WriteTo.Console()
            .Enrich.FromLogContext()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCultureReaderMiddleware();

app.UseAuthorization();

app.MapControllers();

app.UseErrorLogging();

app.Run();
