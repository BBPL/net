using Microsoft.Extensions.Caching.Memory;
using Shape.Weather.Common.Models.Response;
using Shape.Weather.Models.Enums;

namespace WeatherShape.Business.Interfaces
{
    public interface IWeatherHandler
    {
        Task<LocationResponse> GetLocation(ICacheEntry cacheEntry, TempUnitEnum unitMapped, int cityId);
        Task<ForecastResponse> GetForecast(ICacheEntry cacheEntry, int cityId);

    }
}