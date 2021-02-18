using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using SocialNetwork.Application.Activities.Models;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Mappings;
using SocialNetwork.Application.Common.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Activities.Queries
{
    public class GetActivitiesWithPaginationQuery : IRequest<PaginatedList<ActivityDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class GetActivitiesWithPaginationQueryHandler : IRequestHandler<GetActivitiesWithPaginationQuery, PaginatedList<ActivityDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetActivitiesWithPaginationQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<ActivityDto>> Handle(GetActivitiesWithPaginationQuery request, CancellationToken cancellationToken)
        {
            return await _context.Activities.OrderBy(x => x.Title)
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}