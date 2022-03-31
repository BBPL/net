namespace WeatherShape.Models
{
    public class Location
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public double TemperatureMin { get; set; }
        public double TemperatureMax { get; set; }
        public string Unit { get; set; } = string.Empty;

    }
}