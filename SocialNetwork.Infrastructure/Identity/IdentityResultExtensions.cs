using Microsoft.AspNetCore.Identity;
using SocialNetwork.Application.Common.Models;
using System.Linq;

namespace SocialNetwork.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static ObjectResult ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? ObjectResult.Success()
                : ObjectResult.Failure(result.Errors.Select(e => e.Description));
        }
    }
}