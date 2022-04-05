namespace Shape.Weather.Common.Models.Response
{
    public class LocationResponse
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public WeatherDataResponse WeatherData { get; set; } = new WeatherDataResponse();
    }
}