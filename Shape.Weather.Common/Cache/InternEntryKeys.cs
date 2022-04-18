namespace Shape.Weather.Common.Cache
{
    public static class InternEntryKeys
    {
        public static string GetCityWeather(int cityId)
        {
            return $"GetCityWeather_{cityId}";
        }

        public static string GetCityWeatherByDate(int cityId, DateTime date)
        {
            return $"GetCityWeather_{cityId}_{date.Date}";
        }
    }
}
