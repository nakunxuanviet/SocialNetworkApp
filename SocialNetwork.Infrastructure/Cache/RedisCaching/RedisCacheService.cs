using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Common.Interfaces;
using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Cache.RedisCaching
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public T Get<T>(string key)
        {
            var value = _distributedCache.GetString(key);

            if (value != null)
            {
                return JsonSerializer.Deserialize<T>(value);
            }

            return default;
        }

        public T Set<T>(string key, T value)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),  //sets the time in which the cache would expire starting from the time of insertion (which represents the now).
                SlidingExpiration = TimeSpan.FromMinutes(10)  //time upto which the cache entry shall be valid, before which if a hit occurs on the time shall be extended further.
            };

            _distributedCache.SetString(key, JsonSerializer.Serialize(value), options);

            return value;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            var value = await _distributedCache.GetAsync(key);

            if (value != null)
            {
                return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(value));
            }

            return default;
        }

        public async Task<T> SetAsync<T>(string key, T value)
        {
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),  //sets the time in which the cache would expire starting from the time of insertion (which represents the now).
                SlidingExpiration = TimeSpan.FromMinutes(10)  //time upto which the cache entry shall be valid, before which if a hit occurs on the time shall be extended further.
            };

            var byteValue = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));

            await _distributedCache.SetAsync(key, byteValue, options);

            return value;
        }
    }
}