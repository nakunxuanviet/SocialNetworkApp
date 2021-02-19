using FluentValidation;
using SocialNetwork.Application.Activities.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Activities.Validations
{
    public class DeleteActivityCommandValidator : AbstractValidator<DeleteActivityCommand>
    {
        public DeleteActivityCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}