using Newtonsoft.Json;

namespace Shape.Weather.Models.OpenWeather
{
    public class OpenWeatherForecastResponse
    {
        [JsonProperty("cod")]
        public int ResponseCode { get; set; }

        [JsonProperty("city")]
        public CityResponse City { get; set; } = new CityResponse();

        [JsonProperty("list")]
        public List<DayResponse> WeatherDays { get; set; } = new List<DayResponse>();
        
    }
}
