using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Activities.Models;
using SocialNetwork.Application.Common.Exceptions;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Result;
using SocialNetwork.Domain.Entities.Activities;
using SocialNetwork.Domain.SeedWork;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Activities.Queries
{
    public class GetActivityQueryCachingWithMediatR : IRequest<Result<ActivityDto>>, ICacheableMediatrQuery
    {
        public int Id { get; set; }

        public bool BypassCache { get; set; }
        public string CacheKey => $"Activity-{Id}";
        public TimeSpan? SlidingExpiration { get; set; }
    }

    internal class GetActivityQueryCachingWithMediatRHandler : ApplicationService, IRequestHandler<GetActivityQueryCachingWithMediatR, Result<ActivityDto>>
    {
        private readonly IMapper _mapper;
        public GetActivityQueryCachingWithMediatRHandler(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            _mapper = mapper;
        }

        public async Task<Result<ActivityDto>> Handle(GetActivityQueryCachingWithMediatR request, CancellationToken cancellationToken)
        {
            var repository = UnitOfWork.Repository<Activity>();
            var activity = await repository.FindBy(x => x.Id == request.Id, false).ProjectTo<ActivityDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync(cancellationToken);

            if (activity == null)
                throw new NotFoundException(nameof(Activity), request.Id);

            return Result<ActivityDto>.Success(activity);
        }
    }
}
