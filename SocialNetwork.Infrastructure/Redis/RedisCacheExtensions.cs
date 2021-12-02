//using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Infrastructure.Redis;
using System;

namespace RedisCache.Core
{
    public static class RedisCacheExtensions
    {
        /// <summary>
        /// Adds IRedisCacheService to IServiceCollection.
        /// </summary>
        public static IServiceCollection AddRedisCache(this IServiceCollection services, Action<RedisCacheOptions> setupAction)
        {
            // Install pkg Microsoft.Extensions.Caching.Redis" Version="2.2.0"
            //services.AddDistributedRedisCache(setupAction);

            services.AddSingleton<IRedisCacheService, RedisCacheService>();
            return services;
        }
    }
}
