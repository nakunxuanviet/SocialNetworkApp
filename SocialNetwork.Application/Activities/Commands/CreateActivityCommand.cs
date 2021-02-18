using AutoMapper;
using MediatR;
using SocialNetwork.Application.Common.Exceptions;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models;
using SocialNetwork.Domain.Entities.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Activities.Commands
{
    /// <summary>
    /// Create activity command
    /// </summary>
    public class CreateActivityCommand : IRequest<Result<Unit>>
    {
        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string City { get; set; }

        public string Venue { get; set; }

        public bool IsCancelled { get; set; }
    }

    /// <summary>
    /// Create activity command handler
    /// </summary>
    public class CreateActivityCommandHandler : IRequestHandler<CreateActivityCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;

        public CreateActivityCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            var entity = new Activity
            {
                Title = request.Title,
                Date = request.Date,
                Description = request.Description,
                Category = request.Category,
                City = request.City,
                Venue = request.Venue,
                IsCancelled = request.IsCancelled
            };

            _context.Activities.Add(entity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to create activity");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}