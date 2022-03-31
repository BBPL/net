using WeatherShape.Configuration;
using WeatherShape.Configuration.Models;
using WeatherShape.ExternalClients;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IExternalHttpClient, OpenWeatherHttpClient>();
//builder.Services.InitializeConfigurationProvider(builder.Configuration);
//builder.Services.AddAndValidateConfiguration<OpenWeatherConfiguration>();

    
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
