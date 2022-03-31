using Microsoft.AspNetCore.Mvc;
using Shape.Weather.Common.Cache;
using Shape.Weather.Common.Cache.Enums;
using Shape.Weather.Common.Cache.Interfaces;
using WeatherShape.ExternalClients;
using WeatherShape.Models;
using WeatherShape.Models.OpenWeather;

namespace WeatherShape.Controllers
{
    [ApiController]
    [Route("weather")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly Serilog.ILogger _logger;
        private readonly IExternalHttpClient _httpClient;
        private readonly IShapeMemoryCache _cache;

        public WeatherForecastController(Serilog.ILogger logger, IExternalHttpClient httpClient, IShapeMemoryCache cache )
        {
            _logger = logger;
            _httpClient = httpClient;
            _cache = cache;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get(int cityId)
        {
            var result = await _httpClient.Get(cityId);

            if (result is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet(Name = "Summary")]
        [Route("summary")]
        public async Task<List<Location>> GetSummary(string unit, double temperature, List<int> locationIds)
        {
            var result = new List<Location>();
            foreach (var cityId in locationIds)
            {
                Location? location = await _cache.GetOrCreateAsync(
                        InternEntryKeys.GetCityWeather(cityId), 
                        CacheExpirationEnum.NormalExpiration, 
                        async taskCache =>
                {
                    var response = await _httpClient.Get(cityId);
                    if (response is null || response.Main is null) { return null; }

                    var location = new Location
                    {
                        CityId = response.Id,
                        CityName = response.Name,
                        TemperatureMin = response.Main?.temp_min ?? 0,
                        TemperatureMax = response.Main?.temp_max ?? 0,
                        Unit = unit
                    };

                    return location;
                });

                if (location == null)
                {
                    continue;
                }
                else
                {
                    result.Add(location);
                }
            }


            return result;
        }
    }
}