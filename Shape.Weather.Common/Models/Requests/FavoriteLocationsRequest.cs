using Microsoft.AspNetCore.Mvc;

namespace WeatherShape.Models.Requsts
{
    public class FavoriteLocationsRequest
    {
        [FromQuery(Name = "unit")]
        public string Unit { get; set; } = string.Empty;
        [FromQuery(Name = "temperature")]
        public double Temperature { get; set; }
        [FromQuery(Name = "locations")]
        [CommaSeparated]
        public IEnumerable<int> Locations { get; set; } = new List<int>();
    }
}
