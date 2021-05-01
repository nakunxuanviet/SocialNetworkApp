using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Activities.Models;
using SocialNetwork.Application.Common.Exceptions;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Result;
using SocialNetwork.Domain.Entities.Activities;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Activities.Queries
{
    public class GetActivityDetailQuery : IRequest<Result<ActivityDto>>
    {
        public int Id { get; set; }
    }

    public class GetActivityDetailQueryHandler : IRequestHandler<GetActivityDetailQuery, Result<ActivityDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICacheService _cacheService;

        public GetActivityDetailQueryHandler(IApplicationDbContext context, IMapper mapper, ICacheService cacheService)
        {
            _context = context;
            _mapper = mapper;
            _cacheService = cacheService;
        }

        public async Task<Result<ActivityDto>> Handle(GetActivityDetailQuery request, CancellationToken cancellationToken)
        {
            //// TryGet data from Cache. If not Available pull from DB
            //var cached = _cacheService.Get<Activity>(request.Id.ToString());
            //if (cached != null) return cached;
            //else
            //{
            //    // Get data from database
            //    var result = new Result<ActivityDto>();

            //    // insert into cache for future calls
            //    return _cacheService.Set<Activity>(request.Id.ToString(), result);
            //}

            var activity = await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                    //new { currentUsername = _userAccessor.GetUsername() })
                    .FirstOrDefaultAsync(x => x.Id == request.Id);
            if (activity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            return Result<ActivityDto>.Success(activity);
        }
    }
}