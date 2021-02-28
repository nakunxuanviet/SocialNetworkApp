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

        public GetActivityDetailQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<ActivityDto>> Handle(GetActivityDetailQuery request, CancellationToken cancellationToken)
        {
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