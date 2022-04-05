using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Shape.Weather.Common.Cache.Enums;
using Shape.Weather.Common.Cache.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherShape.Configuration.Models.CacheConfiguration;

namespace Shape.Weather.Common.Cache
{
    public class ShapeMemoryCache : MemoryCache, IShapeMemoryCache
    {
        private readonly ConcurrentDictionary<object, DateTime> _entriesTimes;

        private IOptionsMonitor<CacheEntriesConfiguration> CacheConfigurationMonitor { get; }

        public ShapeMemoryCache(IOptions<MemoryCacheOptions> memoryCacheOptions
            , IOptionsMonitor<CacheEntriesConfiguration> cacheConfigurationMonitor) : base(memoryCacheOptions)
        {
            CacheConfigurationMonitor = cacheConfigurationMonitor;
            _entriesTimes = new ConcurrentDictionary<object, DateTime>();
        }


        /// <summary>
        /// Extends base built-in GetOrCreate method by applying configured options to entry before initializing it
        /// </summary>
        /// <typeparam name="TItem">Type of stored object</typeparam>
        /// <param name="key">Object (Guid for example) used as key for creating and retrieving entry</param>
        /// <param name="expirationPreset">Preset for options to be used in initializing entry</param>
        /// <param name="getDataFunc">Func to be used to retrieve the data if it does not exists in cache or when it expired</param>
        /// <param name="shouldRefreshFunc">Input = DateTimeUtc of creation of entry</param>
        /// <param name="correlatedKeys">List of correlated cache keys that should also be refreshed if shouldRefreshFunc is true</param>
        /// <returns></returns>s
        public TItem GetOrCreate<TItem>(object key, CacheExpirationEnum expirationPreset, Func<ICacheEntry, TItem> getDataFunc,
            Func<DateTime, bool>? shouldRefreshFunc = null, List<object>? correlatedKeys = null)
        {
            var options = GetOptionsBasedOnExpirationPreset(expirationPreset);
            if (shouldRefreshFunc != null && _entriesTimes.TryGetValue(key, out var dateCreated))
            {
                if (shouldRefreshFunc(dateCreated))
                {
                    Remove(key);
                    if (correlatedKeys != null)
                    {
                        correlatedKeys.ForEach(x => Remove(x));
                    }
                }
            }
            return this.GetOrCreate<TItem>(key, arg =>
            {
                _entriesTimes.AddOrUpdate(key, DateTime.UtcNow, (key, value) => DateTime.UtcNow);
                arg.SetOptions(options);
                return getDataFunc(arg);
            });
        }

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
        public async Task<TItem> GetOrCreateAsync<TItem>(object key, CacheExpirationEnum expirationPreset,
            Func<ICacheEntry, Task<TItem>> getDataFunc, Func<DateTime, Task<bool>>? shouldRefreshFunc = null,
            List<object>? correlatedKeys = null)
        {
            var options = GetOptionsBasedOnExpirationPreset(expirationPreset);
            if (_entriesTimes.TryGetValue(key, out var dateCreated))
            {
                if (shouldRefreshFunc != null && await shouldRefreshFunc(dateCreated))
                {
                    Remove(key);
                    if (correlatedKeys != null)
                    {
                        correlatedKeys.ForEach(x => Remove(x));
                    }
                }
            }
            return await this.GetOrCreateAsync<TItem>(key, arg =>
            {
                _entriesTimes.AddOrUpdate(key, DateTime.UtcNow, (key, value) => DateTime.UtcNow);
                arg.SetOptions(options);
                return getDataFunc(arg);
            });
        }

        /// <summary>
        /// Extends built-in Set method by applying configured options to entry before initializing it
        /// </summary>
        /// <typeparam name="TItem">Type of stored object</typeparam>
        /// <param name="key">Object (Guid for example) used as key for creating and retrieving entry</param>
        /// <param name="value">Value to be put into cache</param>
        /// <param name="expirationPreset">Preset for options to be used in initializing entry</param>
        /// <returns></returns>
        public TItem Set<TItem>(object key, TItem value, CacheExpirationEnum expirationPreset)
        {
            _entriesTimes.AddOrUpdate(key, DateTime.UtcNow, (key, value) => DateTime.UtcNow);
            return this.Set<TItem>(key, value, GetOptionsBasedOnExpirationPreset(expirationPreset));
        }

        /// <summary>
        /// Retrieves MemoryCacheEntry relevant to ExpirationEnum value
        /// </summary>
        /// <returns></returns>
        private MemoryCacheEntryOptions GetOptionsBasedOnExpirationPreset(CacheExpirationEnum expirationPreset)
        {
            switch (expirationPreset)
            {
                case CacheExpirationEnum.NeverRemove:
                    return new MemoryCacheEntryOptions()
                    {
                        Priority = CacheItemPriority.NeverRemove,
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12)
                    };

                case CacheExpirationEnum.FastExpiration:
                    return CacheConfigurationMonitor
                        .CurrentValue
                        .FastExpirationCacheEntryConfiguration
                        .GetOptions();

                case CacheExpirationEnum.NormalExpiration:
                    return CacheConfigurationMonitor
                        .CurrentValue
                        .NormalExpirationCacheEntryConfiguration
                        .GetOptions();

                case CacheExpirationEnum.SlowExpiration:
                    return CacheConfigurationMonitor
                        .CurrentValue
                        .SlowExpirationCacheEntryConfiguration
                        .GetOptions();
            }

            //By default we will just use Low priority ites that will be first to remove
            return new MemoryCacheEntryOptions()
            {
                Priority = CacheItemPriority.Low
            };
        }
    }
}
