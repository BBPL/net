using WeatherShape.Configuration.Models;
using Serilog;
using WeatherShape;
using Shape.Weather.Common.Cache.Interfaces;
using Shape.Weather.Common.Cache;
using WeatherShape.Configuration.Models.CacheConfiguration;
using WeatherShape.Business.Interfaces;
using WeatherShape.Business;
using Shape.Weather.ExternalClients;
using Serilog.Sinks.Elasticsearch;
using Shape.Weather.Common.Configuration.Models;
using System.Reflection;
using System.Collections.Specialized;

//namespace WeatherShape
//{

//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();
//        }

//        public static IHostBuilder CreateHostBuilder(string[] args) =>
//            Host.CreateDefaultBuilder(args)
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                });
//    }
//}


var builder = WebApplication.CreateBuilder(args);
ConfigureLogs();
builder.Services.AddSingleton(Log.Logger);

//builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console());
//builder.Host.UseSerilog((ctx, lc) => lc.Enrich.FromLogContext()
//                                       .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))));

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
builder.Services.AddAndValidateConfiguration<ElasticConfiguration>();
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


void ConfigureLogs()
{
    // Get the configuration 
    var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
            .Build();


    Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithCorrelationId()
        .WriteTo.Debug()
        .WriteTo.Console()
        .WriteTo.Elasticsearch(ConfigureELS(configuration))
        .ReadFrom.Configuration(configuration)
        .CreateLogger();
    Serilog.Debugging.SelfLog.Enable(Console.Error);

}

ElasticsearchSinkOptions ConfigureELS(IConfigurationRoot configuration)
{
    var uri = configuration["ElasticConfiguration:ElasticUrl"];

    return new ElasticsearchSinkOptions(new Uri(uri))
    {
        AutoRegisterTemplate = true,
        IndexFormat = $"weathershape-dev-{DateTime.UtcNow:yyyy-MM}",
        FailureCallback = x => Console.WriteLine($"Issue connecting to Elastic {x.MessageTemplate}"),
        EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                       EmitEventFailureHandling.WriteToFailureSink |
                                       EmitEventFailureHandling.RaiseCallback,

    };
}