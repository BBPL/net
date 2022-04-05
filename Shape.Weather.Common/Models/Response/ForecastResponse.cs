using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shape.Weather.Common.Models.Response
{
    public class ForecastResponse
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public Dictionary<DateTime, List<WeatherForecastDataResponse>> WeatherData { get; set; } = new Dictionary<DateTime, List<WeatherForecastDataResponse>>();
    }
}
