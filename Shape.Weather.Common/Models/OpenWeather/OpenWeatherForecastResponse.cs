using Newtonsoft.Json;

namespace Shape.Weather.Models.OpenWeather
{
    public class OpenWeatherForecastResponse
    {
        [JsonProperty("city")]
        public CityResponse City { get; set; }
        [JsonProperty("list")]
        public List<DayResponse> WeatherDays { get; set; } = new List<DayResponse>();
        
    }
}
