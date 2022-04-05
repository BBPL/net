using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Shape.Weather.Models.OpenWeather;
using WeatherShape.Configuration.Models;

namespace Shape.Weather.ExternalClients
{
    public class OpenWeatherHttpClient : IExternalHttpClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenWeatherConfiguration _configuration;

        public OpenWeatherHttpClient(IServiceProvider serviceProvider, IOptionsMonitor<OpenWeatherConfiguration> optionsMonitor)
        {
            _httpClient = new HttpClient();
            _configuration = optionsMonitor.CurrentValue;
        }

        /// <summary>
        /// Request current weather for a given city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public async Task<OpenWeatherLocatonResponse> GetLocation(int cityId)
        {
            var url = string.Format(_configuration.LocationUri, cityId, _configuration.ApiKey);
            var response = await _httpClient.GetAsync(url);

            if (response.Content is null)
            {
                return new OpenWeatherLocatonResponse();
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OpenWeatherLocatonResponse>(content);
            if (result == null)
            {
                return new OpenWeatherLocatonResponse();
            }

            return result;
        }

        /// <summary>
        /// Request forecast for a specific city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public async Task<OpenWeatherForecastResponse> GetForecast(int cityId)
        {
            var url = string.Format(_configuration.ForecastUri, cityId, _configuration.ApiKey);
            var response = await _httpClient.GetAsync(url);

            if (response.Content is null)
            {
                return new OpenWeatherForecastResponse();
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OpenWeatherForecastResponse>(content);
            if (result == null)
            {
                return new OpenWeatherForecastResponse();
            }

            return result;
        }
    }
}
