using Newtonsoft.Json;

namespace Shape.Weather.Models.OpenWeather
{
    public class CityResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}