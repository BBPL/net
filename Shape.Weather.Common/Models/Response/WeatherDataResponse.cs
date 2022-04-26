using Shape.Weather.Models.Enums;

namespace Shape.Weather.Common.Models.Response
{
    public class WeatherDataResponse
    {
        public double Temperature { get; set; }
        public string TempUnit { get; set; }
    }
}