using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherShape.Configuration.Models;

namespace Shape.Weather.Common.Configuration.Models
{
    public class ElasticConfiguration : Validatable
    {
        public string ElasticUrl { get; set; } = string.Empty;
    }
}
