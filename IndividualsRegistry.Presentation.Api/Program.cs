using FluentValidation;
using IndividualsRegistry.Application;
using IndividualsRegistry.Domain.Contracts;
using IndividualsRegistry.Infrastructure.Data;
using IndividualsRegistry.Infrastructure.Models.Configuration;
using IndividualsRegistry.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at
// https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var applicationAssembly = typeof(AssemblyReference).Assembly;
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(applicationAssembly));
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddValidatorsFromAssemblyContaining<ApplicationMarker>();
builder.Services.AddLocalization(x => x.ResourcesPath = "Resources");

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IIndividualsRepository, IndividualsRepository>();

var connStr = builder.Configuration.GetSection(nameof(ConnectionStrings)).Get<ConnectionStrings>();
builder.Services.AddDbContext<IndividualsDbContext>(opt => opt.UseSqlServer(connStr!.MainDb));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
