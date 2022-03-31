using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using WeatherShape.Configuration.Models;
using WeatherShape.Models;
using WeatherShape.Models.OpenWeather;

namespace WeatherShape.ExternalClients
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

        public async Task<OpenWeatherResponse?> Get(int cityId)
        {
            var url = string.Format(_configuration.Uri, cityId, _configuration.ApiKey);
            var response = await _httpClient.GetAsync(url);

            if (response.Content is null)
            {
                return null;
            }
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OpenWeatherResponse>(content);

            return result;
        }
    }
}
