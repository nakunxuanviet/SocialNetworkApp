using SocialNetwork.Domain.Shared.ActionResult;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        string GetUserIdentity();

        string GetUserName();

        /// <summary>
        /// Get user name.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<string> GetUserNameAsync(string userId);

        /// <summary>
        /// Registration new user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);

        /// <summary>
        /// Delete user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Result> DeleteUserAsync(string userId);
    }
}