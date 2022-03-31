namespace Shape.Weather.Common.Cache.Enums
{
    public enum CacheExpirationEnum
    {
        /// <summary>
        /// Fast expiration items are to be accessed near realtime
        /// </summary>
        FastExpiration,
        /// <summary>
        /// Normal expiration items will be updated from time to time
        /// </summary>
        NormalExpiration,
        /// <summary>
        /// Slow expiration items are almost never updated
        /// </summary>
        SlowExpiration,
        /// <summary>
        /// Those items by rule will never be removed from cache, so it's especially important
        /// to explicitly manage their lifetime.
        /// For memory reasons it will have expiration time of about 12 hrs.
        /// </summary>
        NeverRemove
    }
}