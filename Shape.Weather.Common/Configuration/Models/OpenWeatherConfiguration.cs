using System.ComponentModel.DataAnnotations;

namespace WeatherShape.Configuration.Models
{
    public class OpenWeatherConfiguration : Validatable
    {
        public OpenWeatherConfiguration()
        {
            Uri = "";
            ApiKey = "";
        }

        [Required]
        public string Uri { get; set; }

        [Required]
        public string ApiKey { get; set; }
    }
}
