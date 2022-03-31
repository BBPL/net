namespace WeatherShape.Configuration.Models.CacheConfiguration
{
    public class CacheEntriesConfiguration : Validatable
    {
        /// <summary>
        /// Fast expiration items are to be accessed near realtime
        /// </summary>
        public FastExpirationCacheEntryConfiguration FastExpirationCacheEntryConfiguration { get; set; }
        /// <summary>
        /// Normal expiration items will be updated from time to time
        /// </summary>
        public NormalExpirationCacheEntryConfiguration NormalExpirationCacheEntryConfiguration { get; set; }
        /// <summary>
        /// Slow expiration items are almost never updated
        /// </summary>
        public SlowExpirationCacheEntryConfiguration SlowExpirationCacheEntryConfiguration { get; set; }
    }
}
