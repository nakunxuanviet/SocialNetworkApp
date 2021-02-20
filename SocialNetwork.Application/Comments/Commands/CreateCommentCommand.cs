using MediatR;
using SocialNetwork.Application.Comments.Models;
using SocialNetwork.Application.Common.Models.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Comments.Commands
{
    public class CreateCommentCommand : IRequest<Result<CommentDto>>
    {
        public string Body { get; set; }
        public int ActivityId { get; set; }
    }

    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentDto>>
    {
        public Task<Result<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}