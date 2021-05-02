using MediatR;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Domain.Entities.Activities;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Activities.Queries
{
    public class GetActivityDetailWithRedisQuery : IRequest<Activity>
    {
        public int Id { get; set; }
    }

    public class GetActivityDetailWithRedisQueryHandler : IRequestHandler<GetActivityDetailWithRedisQuery, Activity>
    {
        private readonly IApplicationDbContext _context;
        private readonly IRedisCacheService _cacheService;

        public GetActivityDetailWithRedisQueryHandler(IRedisCacheService cacheService, IApplicationDbContext context)
        {
            _cacheService = cacheService;
            _context = context;
        }

        public async Task<Activity> Handle(GetActivityDetailWithRedisQuery request, CancellationToken cancellationToken)
        {
            // TryGet data from Cache. If not Available pull from DB
            var cached = await _cacheService.GetAsync<Activity>(request.Id.ToString());
            if (cached != null) return cached;
            else
            {
                // Get data from database
                var result = await _context.Activities.FindAsync(request.Id);

                // insert into cache for future calls
                return await _cacheService.SetAsync<Activity>(request.Id.ToString(), result);
            }
        }
    }
}