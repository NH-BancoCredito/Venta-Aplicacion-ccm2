using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Steeltoe.Extensions.Configuration.ConfigServer;
using System.Runtime.CompilerServices;
using Venta.Api;
using Venta.Api.Configurations;
using Venta.Api.Middleware;
using Venta.Application;
using Venta.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddConfigServer(
    LoggerFactory.Create(builder =>
    {
        builder.AddConsole();
    })
    );

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Capa de aplicacion
builder.Services.AddApplication();

//Capa de infra

//var connectionString = builder.Configuration.GetConnectionString("dbVenta-cnx");
var connectionString = builder.Configuration["dbVenta-cnx"];
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddAthenticationByJWT();
builder.Services.AddHealthCheckConfiguration(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseHealthCheckConfiguration();


//Adicionar middleware customizado para tratar las excepciones
app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();
