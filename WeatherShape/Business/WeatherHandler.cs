using Microsoft.Extensions.Caching.Memory;
using Shape.Weather.Common.Models.Response;
using Shape.Weather.ExternalClients;
using Shape.Weather.Models.Enums;
using WeatherShape.Business.Interfaces;
using WeatherShape.Converters;

namespace WeatherShape.Business
{
    /// <summary>
    /// Handler for retrieving weather data
    /// </summary>
    public class WeatherHandler : IWeatherHandler
    {
        private readonly IExternalHttpClient _httpClient;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="httpClient"></param>
        public WeatherHandler(IExternalHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Read current weather data for a given city
        /// </summary>
        /// <param name="cacheEntry"></param>
        /// <param name="unitMapped"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<LocationResponse> GetLocation(ICacheEntry cacheEntry, TempUnitEnum unitMapped, int cityId)
        {
            var openWeatherResponse = await _httpClient.GetLocation(cityId);

            double temperatureK = openWeatherResponse.WeatherData.temp;
            var temperature = unitMapped switch
            {
                TempUnitEnum.Celsius => TemperatureConverter.FromKelvinToCelsius(temperatureK),
                TempUnitEnum.Fahrenheit => TemperatureConverter.FromKelvinToFahrenheit(temperatureK),
                _ => throw new NotImplementedException()
            };

            var response = new LocationResponse
            {
                CityId = cityId,
                CityName = openWeatherResponse.Name,
                WeatherData = new WeatherDataResponse
                {
                    Temperature = temperature,
                    TempUnit = unitMapped
                }
            };

            return response;
        }

        /// <summary>
        /// Read forecast for a specific location for the next 5 days, returns weather 
        /// data for every 3 hours. Weather data is grouped into days.
        /// </summary>
        /// <param name="cacheEntry"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public async Task<ForecastResponse> GetForecast(ICacheEntry cacheEntry, int cityId)
        {
            var forecastResponse = await _httpClient.GetForecast(cityId);

            var weatherDataResponses = new List<WeatherForecastDataResponse>();
            foreach (var weatherDay in forecastResponse.WeatherDays)
            {
                var temperatureK = weatherDay.WeatherData.temp;
                var weatherData = new WeatherForecastDataResponse
                {
                    TemperatureF = TemperatureConverter.FromKelvinToFahrenheit(temperatureK),
                    TemperatureC = TemperatureConverter.FromKelvinToCelsius(temperatureK),
                    DateTime = DateTimeOffset.FromUnixTimeSeconds(weatherDay.TimestampUtc).UtcDateTime
                };
                weatherDataResponses.Add(weatherData);
            }

            var groupedWeatherData = weatherDataResponses.GroupBy(x => x.DateTime.Date).ToDictionary(x => x.Key, x => x.ToList());

            var result = new ForecastResponse
            {
                CityId = forecastResponse.City.Id,
                CityName = forecastResponse.City.Name,
                WeatherData = groupedWeatherData
            };

            return result;
        }
    }
}
