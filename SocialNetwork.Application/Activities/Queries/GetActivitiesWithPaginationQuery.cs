﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using SocialNetwork.Application.Activities.Models;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Mappings;
using SocialNetwork.Application.Common.Models.Paged;
using SocialNetwork.Application.Common.Models.Result;
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
        public DateTime StartDate { get; set; }
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
                   .OrderBy(d => d.Date)
                   .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider)
                   //new { currentUsername = _userAccessor.GetUsername() })
                   .AsQueryable();

            if (request.StartDate != DateTime.MinValue)
            {
                query = query.Where(d => d.Date >= request.StartDate);
            }

            return Result<PaginatedList<ActivityDto>>.Success(await query
                .PaginatedListAsync(request.PageNumber, request.PageSize));
        }
    }
}