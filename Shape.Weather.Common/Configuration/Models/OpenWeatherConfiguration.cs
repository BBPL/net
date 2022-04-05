using System.ComponentModel.DataAnnotations;

namespace WeatherShape.Configuration.Models
{
    public class OpenWeatherConfiguration : Validatable
    {
        public OpenWeatherConfiguration()
        {
            LocationUri = "";
            ForecastUri = "";
            ApiKey = "";
        }

        [Required]
        public string LocationUri { get; set; }

        [Required]
        public string ForecastUri { get; set; }

        [Required]
        public string ApiKey { get; set; }
    }
}
