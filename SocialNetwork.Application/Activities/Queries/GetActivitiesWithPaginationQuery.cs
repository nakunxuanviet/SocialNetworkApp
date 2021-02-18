using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using SocialNetwork.Application.Activities.Models;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Mappings;
using SocialNetwork.Application.Common.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Activities.Queries
{
    public class GetActivitiesWithPaginationQuery : PagingParams, IRequest<Result<PaginatedList<ActivityDto>>>
    {
        public bool IsGoing { get; set; }
        public bool IsHost { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
    }

    public class GetActivitiesWithPaginationQueryHandler : IRequestHandler<GetActivitiesWithPaginationQuery, Result<PaginatedList<ActivityDto>>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetActivitiesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<PaginatedList<ActivityDto>>> Handle(GetActivitiesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Activities
                   .Where(d => d.Date >= request.StartDate)
                   .OrderBy(d => d.Date)
                   .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                   //new { currentUsername = _userAccessor.GetUsername() })
                   .AsQueryable();

            return Result<PaginatedList<ActivityDto>>.Success(await query
                .PaginatedListAsync(request.PageNumber, request.PageSize));
        }
    }
}