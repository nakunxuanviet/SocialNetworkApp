﻿using Microsoft.AspNetCore.Identity;
using SocialNetwork.Application.Common.Models;
using System.Linq;

namespace SocialNetwork.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}