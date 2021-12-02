using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Domain.Entities.ApplicationUsers;
using SocialNetwork.Domain.Shared.ActionResult;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private IHttpContextAccessor _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityService(IHttpContextAccessor context, UserManager<ApplicationUser> userManager)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager;
        }

        public string GetUserIdentity()
        {
            return _context.HttpContext.User.FindFirst("sub").Value;
        }

        public string GetUserId()
        {
            var user = _context.HttpContext.User;
            return user == null ? "" : user.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
        }

        public string GetUserName()
        {
            return _context.HttpContext.User.Identity.Name;
            //var user = _context.HttpContext.User;
            //return user == null ? "" : user.FindFirstValue(ClaimTypes.Name) ?? "";
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public bool IsAuthenticated()
        {
            return _context.HttpContext.User.Identity.IsAuthenticated;
        }

        public async Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password)
        {
            var user = new ApplicationUser
            {
                UserName = userName,
                Email = userName,
            };

            var result = await _userManager.CreateAsync(user, password);

            return (ToIdentityResult(result), user.Id);
        }

        public async Task<Result> DeleteUserAsync(string userId)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            if (user != null)
            {
                return await DeleteUserAsync(user);
            }

            return Result.Success();
        }

        public async Task<Result> DeleteUserAsync(ApplicationUser user)
        {
            var result = await _userManager.DeleteAsync(user);

            return ToIdentityResult(result);
        }

        private Result ToIdentityResult(IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description).ToList());
        }
    }
}