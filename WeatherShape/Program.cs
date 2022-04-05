using WeatherShape.Configuration.Models;
using Serilog;
using WeatherShape;
using Shape.Weather.Common.Cache.Interfaces;
using Shape.Weather.Common.Cache;
using WeatherShape.Configuration.Models.CacheConfiguration;
using WeatherShape.Business.Interfaces;
using WeatherShape.Business;
using Shape.Weather.ExternalClients;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

// Add services to the container.
builder.Services.AddControllers(o => o.Conventions.Add(new CommaSeparatedQueryStringConvention()));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IExternalHttpClient, OpenWeatherHttpClient>();
builder.Services.AddScoped<IWeatherHandler, WeatherHandler>();
builder.Services.AddSingleton<IShapeMemoryCache, ShapeMemoryCache>();
builder.Services.InitializeConfigurationProvider(builder.Configuration, true);
builder.Services.AddAndValidateConfiguration<OpenWeatherConfiguration>();
builder.Services.AddAndValidateConfiguration<CacheEntriesConfiguration>();
builder.Services.AddSwaggerGenNewtonsoftSupport();

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
