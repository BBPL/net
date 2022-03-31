using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shape.Weather.Common.Cache
{
    public static class InternEntryKeys
    {
        public static string GetCityWeather(int cityId)
        {
            return $"GetCityWeather_{cityId}";
        }
    }
}
