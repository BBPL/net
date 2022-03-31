using WeatherShape.Models.OpenWeather;

namespace WeatherShape.ExternalClients
{
    public interface IExternalHttpClient
    {
        Task<OpenWeatherResponse?> Get(int cityId);
    }
}
