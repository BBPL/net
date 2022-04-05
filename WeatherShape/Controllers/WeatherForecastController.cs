using Microsoft.AspNetCore.Mvc;
using Shape.Weather.Common.Cache;
using Shape.Weather.Common.Cache.Enums;
using Shape.Weather.Common.Cache.Interfaces;
using Shape.Weather.Common.Models.Response;
using Shape.Weather.Models.Enums;
using WeatherShape.Business.Interfaces;
using WeatherShape.Models.Requsts;

namespace WeatherShape.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("weather/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly Serilog.ILogger _logger;
        private readonly IShapeMemoryCache _cache;
        private readonly IWeatherHandler _weatherHandler;

        public WeatherForecastController(Serilog.ILogger logger,
            IShapeMemoryCache cache,
            IWeatherHandler weatherHandler
            )
        {
            _logger = logger;
            _cache = cache;
            _weatherHandler = weatherHandler;
        }

        /// <summary>
        /// Get current information for a given location by city id and temperature unit. The method ensures 
        /// to verify if we do not have such city in our cache already. 
        /// If the given city is in our cache we return it, otherwise we get the handler to retrieve the
        /// necessary information
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("summary")]
        [SeparatedQueryString]
        public async Task<ActionResult> GetSummary([FromQuery]FavoriteLocationsRequest request)
        {
            var result = new List<LocationResponse>();
            var date = DateTime.UtcNow.Date;
            _ = Enum.TryParse<TempUnitEnum>(request.Unit, out TempUnitEnum unitMapped);

            foreach (var cityId in request.Locations)
            {
                LocationResponse? location = await _cache.GetOrCreateAsync(
                        InternEntryKeys.GetCityWeather(cityId),
                        CacheExpirationEnum.SlowExpiration,
                        async taskCache => await _weatherHandler.GetLocation(taskCache, unitMapped, cityId));

                if (location == null)
                {
                    continue;
                }
                else
                {
                    result.Add(location);
                }
            }

            result = result.Where(x => x.WeatherData.Temperature > request.Temperature).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Get forecast for a given location by city id. The method ensures to verify if we do not have
        /// such city in our cache already. 
        /// If the given city is in our cache we return it, otherwise we get the handler to retrieve the
        /// necessary information
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("locations/{cityId:int}")]
        public async Task<ActionResult> GetLocationInformation([FromRoute] int cityId)
        {
            var date = DateTime.UtcNow.Date;
            
            ForecastResponse? weatherForecast = await _cache.GetOrCreateAsync(
                InternEntryKeys.GetCityWeatherByDate(cityId, date),
                CacheExpirationEnum.SlowExpiration,
                async cacheEntry => await _weatherHandler.GetForecast(cacheEntry, cityId));

            return Ok(weatherForecast);
        }

    
    }
}