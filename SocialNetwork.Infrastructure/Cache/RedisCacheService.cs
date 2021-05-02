using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Cache;
using System;
using System.Text;
using System.Text.Json;

namespace SocialNetwork.Infrastructure.Cache
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly CacheConfiguration _cacheConfig;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public RedisCacheService(IDistributedCache distributedCache, IOptions<CacheConfiguration> cacheConfig)
        {
            _distributedCache = distributedCache;
            _cacheConfig = cacheConfig.Value;
            if (_cacheConfig != null)
            {
                _cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(_cacheConfig.AbsoluteExpirationInHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes)
                };
            }
        }

        public bool TryGet<T>(string cacheKey, out T value)
        {
            byte[] data = _distributedCache.Get(cacheKey);

            if (data == null)
            {
                value = default;
                return false;
            }

            value = JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(data));
            return true;
        }

        public T Set<T>(string cacheKey, T value)
        {
            _distributedCache.SetString(cacheKey, JsonSerializer.Serialize(value), _cacheOptions);

            return value;
        }

        public void Remove(string cacheKey)
        {
            _distributedCache.Remove(cacheKey);
        }
    }
}