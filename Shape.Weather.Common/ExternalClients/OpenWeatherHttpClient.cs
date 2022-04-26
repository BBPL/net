using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using Shape.Weather.Models.OpenWeather;
using System.Security.Authentication;
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
        public async Task<OpenWeatherLocatonResponse?> GetLocation(int cityId)
        {
            _logger.Information("[OpenWeather] - Requesting information for cityId {CityId}", cityId);
            var url = string.Format(_configuration.LocationUri, cityId, _configuration.ApiKey);
            var response = await _httpClient.GetAsync(url);

            if (response.Content is null)
            {
                _logger.Warning("[OpenWeather] - Could not find information about cityId {CityId}", cityId);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OpenWeatherLocatonResponse>(content);
            if (result == null)
            {
                var errorMsg = string.Format("[OpenWeather] - Could not deserialize answer for cityId {CityId}", cityId);
                _logger.Warning(errorMsg);
                throw new Exception(errorMsg);
            }

            if (!IsSuccessfullStatusCode(result.ResponseCode, cityId))
            {
                return null;
            }
           
            _logger.Information("[OpenWeather] - Returning information for cityId {CityId}", cityId);
            return result;
        }

        /// <summary>
        /// Request forecast for a specific city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public async Task<OpenWeatherForecastResponse?> GetForecast(int cityId)
        {
            _logger.Information("[OpenWeather] - Requesting forecast for cityId {CityId}", cityId);
            var url = string.Format(_configuration.ForecastUri, cityId, _configuration.ApiKey);
            var response = await _httpClient.GetAsync(url);

            if (response.Content is null)
            {
                _logger.Warning("[OpenWeather] - Could not find forecast about cityId {CityId}", cityId);
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OpenWeatherForecastResponse>(content);
            if (result == null)
            {
                var errorMsg = string.Format("[OpenWeather] - Could not deserialize answer for forecast of cityId {CityId}", cityId);
                _logger.Error(errorMsg);
                throw new Exception(errorMsg);
            }

            if (!IsSuccessfullStatusCode(result.ResponseCode, cityId))
            {
                return null;
            }

            _logger.Information("[OpenWeather] - Returning forecast for cityId {CityId}", cityId);
            return result;
        }

        private bool IsSuccessfullStatusCode(int responseCode, int cityId)
        {
            switch (responseCode)
            {
                case 200:
                    return true;
                case 401:
                    var unauthenticatedMsg = "[OpenWeather] - Failed authentication";
                    _logger.Error(unauthenticatedMsg);
                    throw new AuthenticationException(unauthenticatedMsg);
                case 404:
                case 400:
                    _logger.Information("[OpenWeather] - Failed fetching city {cityId}", cityId);
                    return false;
                case 500:
                case 503:
                    var errorMsg = "[OpenWeather] - The external API failed the request";
                    _logger.Error(errorMsg);
                    throw new Exception(errorMsg);
                default:
                    return true;
            }

        }
    }
}
