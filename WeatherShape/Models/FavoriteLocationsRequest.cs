using WeatherShape.Models.Enums;

namespace WeatherShape.Models
{
    public class FavoriteLocationsRequest
    {
        public TempUnitEnum Unit { get; set; }
        public double Temperature { get; set; }
        public List<int> Locations { get; set; } = new List<int>();
    }
}
