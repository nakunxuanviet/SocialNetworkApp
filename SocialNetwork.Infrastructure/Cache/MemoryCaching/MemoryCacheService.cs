using Microsoft.Extensions.Caching.Memory;
using SocialNetwork.Application.Common.Interfaces;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Infrastructure.Cache.MemoryCaching
{
    public class MemoryCacheService : IMemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public string Get(string key)
        {
            _memoryCache.TryGetValue(key, out string value);
            return value;
        }

        public void Set(string key, string value)
        {
            var cacheExpiryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                Priority = CacheItemPriority.High,
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Size = 1024,
            };
            _memoryCache.Set(key, value, cacheExpiryOptions);
        }

        // Example:
        //public async Task<IActionResult> GetAll()
        //{
        //    var cacheKey = "customerList";
        //    if (!memoryCache.TryGetValue(cacheKey, out List<Customer> customerList))
        //    {
        //        customerList = await context.Customers.ToListAsync();
        //        var cacheExpiryOptions = new MemoryCacheEntryOptions
        //        {
        //            AbsoluteExpiration = DateTime.Now.AddMinutes(5),
        //            Priority = CacheItemPriority.High,
        //            SlidingExpiration = TimeSpan.FromMinutes(2)
        //        };
        //        memoryCache.Set(cacheKey, customerList, cacheExpiryOptions);
        //    }
        //    return Ok(customerList);
        //}
    }
}