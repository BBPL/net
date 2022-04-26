
using Shape.Weather.Models.OpenWeather;

namespace Shape.Weather.ExternalClients
{
    public interface IExternalHttpClient
    {
        /// <summary>
        /// Request forecast for a specific city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<OpenWeatherForecastResponse?> GetForecast(int cityId);

        /// <summary>
        /// Request current weather for a given city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        Task<OpenWeatherLocatonResponse?> GetLocation(int cityId);
    }
}
