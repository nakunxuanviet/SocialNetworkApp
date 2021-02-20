using AutoMapper;
using MediatR;
using SocialNetwork.Application.Common.Exceptions;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Result;
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
    /// Update activity command
    /// </summary>
    public class UpdateActivityCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string City { get; set; }

        public string Venue { get; set; }

        public bool IsCancelled { get; set; }
    }

    /// <summary>
    /// Update activity command handler
    /// </summary>
    public class UpdateActivityCommandHandler : IRequestHandler<UpdateActivityCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;

        public UpdateActivityCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(UpdateActivityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            entity.Title = request.Title ?? entity.Title;
            entity.Date = request.Date;
            entity.Description = request.Description ?? entity.Description;
            entity.Category = request.Category ?? entity.Category;
            entity.City = request.City ?? entity.City;
            entity.Venue = request.Venue ?? entity.Venue;
            entity.IsCancelled = request.IsCancelled;

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to update activity");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}