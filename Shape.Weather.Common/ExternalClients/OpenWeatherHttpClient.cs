using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using Shape.Weather.Models.OpenWeather;
using WeatherShape.Configuration.Models;

namespace Shape.Weather.ExternalClients
{
    public class OpenWeatherHttpClient : IExternalHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherConfiguration _configuration;
        private readonly ILogger _logger;

        public OpenWeatherHttpClient(IServiceProvider serviceProvider, IOptionsMonitor<OpenWeatherConfiguration> optionsMonitor)
        {
            _httpClient = new HttpClient();
            _configuration = optionsMonitor.CurrentValue;
            var logger = serviceProvider.GetService<ILogger>();
            _logger = logger ?? throw new NullReferenceException(nameof(logger));
        }

        /// <summary>
        /// Request current weather for a given city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public async Task<OpenWeatherLocatonResponse> GetLocation(int cityId)
        {
            _logger.Information("[OpenWeather] - Requesting information for cityId {CityId}", cityId);
            var url = string.Format(_configuration.LocationUri, cityId, _configuration.ApiKey);
            var response = await _httpClient.GetAsync(url);

            if (response.Content is null)
            {
                _logger.Warning("[OpenWeather] - Could not find information about cityId {CityId}", cityId);
                return new OpenWeatherLocatonResponse();
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OpenWeatherLocatonResponse>(content);
            if (result == null)
            {
                _logger.Warning("[OpenWeather] - Could not deserialize answer for cityId {CityId}", cityId);
                return new OpenWeatherLocatonResponse();
            }

            _logger.Information("[OpenWeather] - Returning information for cityId {CityId}", cityId);
            return result;
        }

        /// <summary>
        /// Request forecast for a specific city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public async Task<OpenWeatherForecastResponse> GetForecast(int cityId)
        {
            _logger.Information("[OpenWeather] - Requesting forecast for cityId {CityId}", cityId);
            var url = string.Format(_configuration.ForecastUri, cityId, _configuration.ApiKey);
            var response = await _httpClient.GetAsync(url);

            if (response.Content is null)
            {
                _logger.Warning("[OpenWeather] - Could not find forecast about cityId {CityId}", cityId);
                return new OpenWeatherForecastResponse();
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OpenWeatherForecastResponse>(content);
            if (result == null)
            {
                _logger.Warning("[OpenWeather] - Could not deserialize answer for forecast of cityId {CityId}", cityId);
                return new OpenWeatherForecastResponse();
            }

            _logger.Information("[OpenWeather] - Returning forecast for cityId {CityId}", cityId);
            return result;
        }
    }
}
