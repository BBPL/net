namespace WeatherShape.Converters
{
    public static class TemperatureConverter
    {
        /// <summary>
        /// Kelvin zero
        /// </summary>
        private static readonly double KelvinZero = 273.15;

        /// <summary>
        /// Converts Kelvin to Celsius
        /// </summary>
        /// <param name="kelvinTemp"></param>
        /// <returns></returns>
        public static double FromKelvinToCelsius(double kelvinTemp)
        {
            return Math.Round((kelvinTemp - KelvinZero), 2);
        }

        /// <summary>
        /// Converts Kelvin to Fahrenheit
        /// </summary>
        /// <param name="kelvinTemp"></param>
        /// <returns></returns>
        public static double FromKelvinToFahrenheit(double kelvinTemp)
        {
            return Math.Round((1.8 * (kelvinTemp - 273) + 32), 2);
        }
    }
}
