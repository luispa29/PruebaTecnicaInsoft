using Application.Ports.Driven;
using Application.Ports.Driving;
using Domain.UseCases;
using Infrastructure.Repositories;
using MapSql.Extensions;
using Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(nameof(AppSettings)));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opciones =>
{
    opciones.SwaggerDoc("v1", new()
    {
        Title = "API Gestión de Citas",
        Version = "v1",
        Description = "Sistema de gestión de citas de mantenimiento vehicular"
    });
});

builder.Services.AddMapSql(builder.Configuration);

builder.Services.AddRouting(opciones => opciones.LowercaseUrls = true);

builder.Services.AddScoped<ICitaRepositorio, CitaRepositorio>();
builder.Services.AddScoped<ICitaServicio, CitaUseCase>();

builder.Services.AddCors(opciones =>
{
    opciones.AddPolicy("PermitirTodo",
        politica => politica
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("PermitirTodo");

app.UseAuthorization();

app.MapControllers();

app.Run();
