using Newtonsoft.Json;

namespace Shape.Weather.Models.OpenWeather
{
    public class OpenWeatherLocatonResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        
        [JsonProperty("main")]
        public Main WeatherData { get; set; } = new Main();
    }
}
