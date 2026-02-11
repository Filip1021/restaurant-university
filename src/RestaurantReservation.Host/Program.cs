using RestaurantReservation.BL.Interfaces;
using RestaurantReservation.BL.Services;
using RestaurantReservation.DAL.Interfaces;
using RestaurantReservation.DAL.Mongo;
using RestaurantReservation.DAL.Repositories;
using RestaurantReservation.BL.Options;
using RestaurantReservation.Host.Dtos;
using RestaurantReservation.Host.Validation;
using RestaurantReservation.Host.Options;
using FluentValidation;
using Serilog;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Text.Json;



var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) =>
    lc.ReadFrom.Configuration(ctx.Configuration)
      .Enrich.FromLogContext()
      .WriteTo.Console());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidator<ReservationRequestDto>, ReservationRequestValidator>();

// Options
builder.Services.Configure<MongoOptions>(builder.Configuration.GetSection("Mongo"));
builder.Services.Configure<ReservationOptions>(builder.Configuration.GetSection("Reservation"));

// Mongo
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var mongo = sp.GetRequiredService<IOptionsMonitor<MongoOptions>>().CurrentValue;
    return new MongoClient(mongo.ConnectionString);
});

builder.Services.AddSingleton<IMongoDatabase>(sp =>
{
    var mongo = sp.GetRequiredService<IOptionsMonitor<MongoOptions>>().CurrentValue;
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongo.DatabaseName);
});

builder.Services.AddSingleton<MongoContext>();

// DAL
builder.Services.AddScoped<IReservationSlotRepository, ReservationSlotRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// BL
builder.Services.AddScoped<IReservationSlotService, ReservationSlotsService>();
builder.Services.AddScoped<IReservationService, ReservationService>();

// HealthChecks (Mongo)
var mongoConn = builder.Configuration["Mongo:ConnectionString"];
builder.Services.AddHealthChecks()
    .AddMongoDb(mongoConn!, name: "mongo");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // OFF

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        context.Response.ContentType = "application/json";
        var response = new
        {
            status = report.Status.ToString(),
            checks = report.Entries.Select(e => new
            {
                name = e.Key,
                status = e.Value.Status.ToString(),
                error = e.Value.Exception?.Message
            })
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
});

app.MapControllers();
app.Run();
