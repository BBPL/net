using Microsoft.Extensions.Caching.Memory;

namespace WeatherShape.Configuration.Models.CacheConfiguration
{
    public class CacheEntryBaseOption : Validatable
    {
        /// <summary>
        /// States in how much time related to entry creation it will expire
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        /// <summary>
        /// States in how much time without any access entry will expire. 
        /// If entry is accessed before that time passes,
        /// It will refresh that timer. Although it will never live longer than specified
        /// in AbsoluteExpiration.
        /// </summary>
        public TimeSpan? SlidingExpiration { get; set; }

        /// <summary>
        /// Helper method that will convert those options to the one used by MemoryCache instance
        /// </summary>
        /// <returns></returns>
        public MemoryCacheEntryOptions GetOptions()
        {
            return new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = this.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = this.SlidingExpiration
            };
        }
    }
}
