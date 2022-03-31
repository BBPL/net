namespace WeatherShape.Models.OpenWeather
{
    public class OpenWeatherResponse
    {
       
        public Main? Main { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
