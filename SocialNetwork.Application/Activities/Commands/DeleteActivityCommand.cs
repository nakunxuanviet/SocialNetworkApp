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
    /// Delete activity command
    /// </summary>
    public class DeleteActivityCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Delete activity command handler
    /// </summary>
    public class DeleteActivityCommandHandler : IRequestHandler<DeleteActivityCommand, Result<Unit>>
    {
        private readonly IApplicationDbContext _context;

        public DeleteActivityCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Unit>> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Activities.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Activity), request.Id);
            }

            _context.Activities.Remove(entity);

            var result = await _context.SaveChangesAsync(cancellationToken) > 0;

            if (!result) return Result<Unit>.Failure("Failed to delete activity");

            return Result<Unit>.Success(Unit.Value);
        }
    }
}