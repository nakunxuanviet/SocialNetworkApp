using System;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface ICacheableMediatrQuery
    {
        // Determines if you want to skip caching and go directly to the database/datastore
        bool BypassCache { get; }
        // Specifies a unique cache key for each similar request
        string CacheKey { get; }
        // Time in hours till which the cache should be held in the memory
        TimeSpan? SlidingExpiration { get; }
    }
}
