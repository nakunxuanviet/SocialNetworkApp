using FluentValidation;
using SocialNetwork.Application.Activities.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Activities.Validations
{
    public class GetActivityDetailQueryValidator : AbstractValidator<GetActivityDetailQuery>
    {
        public GetActivityDetailQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id is required.");
        }
    }
}