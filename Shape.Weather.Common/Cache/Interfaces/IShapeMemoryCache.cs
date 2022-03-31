using Microsoft.Extensions.Caching.Memory;
using Shape.Weather.Common.Cache.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shape.Weather.Common.Cache.Interfaces
{
    public interface IShapeMemoryCache : IMemoryCache
    {
        /// <summary>
        /// Extends base built-in GetOrCreate method by applying configured options to entry before initializing it
        /// </summary>
        /// <typeparam name="TItem">Type of stored object</typeparam>
        /// <param name="key">Object (Guid for example) used as key for creating and retrieving entry</param>
        /// <param name="expirationPreset">Preset for options to be used in initializing entry</param>
        /// <param name="getDataFunc">Func to be used to retrieve the data if it does not exists in cache or when it expired</param>
        /// <param name="shouldRefreshFunc">Input = DateTimeUtc of creation of entry</param>
        /// <param name="correlatedKeys">List of correlated cache keys that should also be refreshed if shouldRefreshFunc is true</param>
        /// <returns></returns>
        TItem GetOrCreate<TItem>(object key, CacheExpirationEnum expirationPreset, Func<ICacheEntry, TItem> getDataFunc,
            Func<DateTime, bool>? shouldRefreshFunc = null, List<object>? correlatedKeys = null);

        /// <summary>
        /// Extends base built-in GetOrCreate method by applying configured options to entry before initializing it
        /// </summary>
        /// <typeparam name="TItem">Type of stored object</typeparam>
        /// <param name="key">Object (Guid for example) used as key for creating and retrieving entry</param>
        /// <param name="expirationPreset">Preset for options to be used in initializing entry</param>
        /// <param name="getDataFunc">Func to be used to retrieve the data if it does not exists in cache or when it expired</param>
        /// <param name="shouldRefreshFunc">Input = DateTimeUtc of creation of entry</param>
        /// <param name="correlatedKeys">List of correlated cache keys that should also be refreshed if shouldRefreshFunc is true</param>
        /// <returns></returns>
        Task<TItem> GetOrCreateAsync<TItem>(object key, CacheExpirationEnum expirationPreset, Func<ICacheEntry,
            Task<TItem>> getDataFunc, Func<DateTime, Task<bool>>? shouldRefreshFunc = null, List<object>? correlatedKeys = null);

        /// <summary>
        /// Extends built-in Set method by applying configured options to entry before initializing it
        /// </summary>
        /// <typeparam name="TItem">Type of stored object</typeparam>
        /// <param name="key">Object (Guid for example) used as key for creating and retrieving entry</param>
        /// <param name="value">Value to be put into cache</param>
        /// <param name="expirationPreset">Preset for options to be used in initializing entry</param>
        /// <returns></returns>
        TItem Set<TItem>(object key, TItem value, CacheExpirationEnum expirationPreset);
    }
}
