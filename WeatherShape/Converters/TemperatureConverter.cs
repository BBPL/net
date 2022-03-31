namespace WeatherShape.Converters
{
    public static class TemperatureConverter
    {

        private static readonly double KelvinZero = 273.15;

        public static double FromKelvinToCelcius(double kelvinTemp)
        {
            return (kelvinTemp - KelvinZero);
        }
    }
}
