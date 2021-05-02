using MediatR;
using Microsoft.EntityFrameworkCore;
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
        private readonly ICacheService _cacheService;

        public GetActivityDetailWithRedisQueryHandler(ICacheService cacheService, IApplicationDbContext context)
        {
            _cacheService = cacheService;
            _context = context;
        }

        public async Task<Activity> Handle(GetActivityDetailWithRedisQuery request, CancellationToken cancellationToken)
        {
            var cached = _cacheService.TryGet<Activity>(request.Id.ToString(), out Activity result);
            if (cached) return result;
            else
            {
                // Get data from database
                var data = await _context.Activities.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

                // insert into cache for future calls
                return _cacheService.Set<Activity>(request.Id.ToString(), data);
            }
        }
    }
}