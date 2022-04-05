using Newtonsoft.Json;

namespace Shape.Weather.Models.OpenWeather
{
    public class DayResponse
    {
        [JsonProperty("dt")]
        public int TimestampUtc { get; set; }

        [JsonProperty("main")]
        public Main WeatherData { get; set; } = new Main();
    }
}