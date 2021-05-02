using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Domain.Entities.Activities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Activities.Queries
{
    public class GetAllActivitieslWithRedisCacheQuery : IRequest<List<Activity>>
    {
        public bool IsGoing { get; set; }
        public bool IsHost { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class GetAllActivitieslWithRedisCacheQueryHandler : IRequestHandler<GetAllActivitieslWithRedisCacheQuery, List<Activity>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IDistributedCache _distributedCache;

        public GetAllActivitieslWithRedisCacheQueryHandler(IApplicationDbContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        public async Task<List<Activity>> Handle(GetAllActivitieslWithRedisCacheQuery request, CancellationToken cancellationToken)
        {
            var cacheKey = "activityList";
            string serializedActivityList;
            List<Activity> activityList;

            // Get data from Redis
            var redisCustomerList = await _distributedCache.GetAsync(cacheKey, cancellationToken);
            if (redisCustomerList != null)
            {
                serializedActivityList = Encoding.UTF8.GetString(redisCustomerList);
                activityList = JsonSerializer.Deserialize<List<Activity>>(serializedActivityList);  // Convert JSON to objects
            }
            else
            {
                activityList = await _context.Activities.ToListAsync(cancellationToken);

                // Set to Redis
                serializedActivityList = JsonSerializer.Serialize(activityList);    // Convert objects as JSON
                redisCustomerList = Encoding.UTF8.GetBytes(serializedActivityList); // Converts the string to a Byte Array. This array will be stored in Redis
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))             // set the expiration time of the cached object
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2)); // This is similar to Absolute Expiration. It expires as a cached object if it not being requested for a defined amount of time period. Note that Sliding Expiration should always be set lower than the absolute expiration.
                await _distributedCache.SetAsync(cacheKey, redisCustomerList, options, cancellationToken);
            }

            return activityList ?? new List<Activity>();
        }
    }
}