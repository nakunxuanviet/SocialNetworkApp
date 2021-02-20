using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SocialNetwork.Infrastructure.Persistence;

namespace SocialNetwork.Infrastructure.Identity
{
    public class IsHostRequirement : IAuthorizationRequirement
    {
    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsHostRequirementHandler(ApplicationDbContext dbContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) return Task.CompletedTask;

            var activityId = _httpContextAccessor.HttpContext?.Request.RouteValues
                .SingleOrDefault(x => x.Key == "id").Value?.ToString();

            //var attendee = _dbContext.ActivityAttendees
            //    .AsNoTracking()
            //    .SingleOrDefaultAsync(x => x.AppUserId == userId && x.ActivityId == activityId)
            //    .Result;

            //if (attendee == null) return Task.CompletedTask;

            //if (attendee.IsHost) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}