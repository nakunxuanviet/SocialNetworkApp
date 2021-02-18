using SocialNetwork.Application.Common.Models;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<(ObjectResult Result, string UserId)> CreateUserAsync(string userName, string password);

        Task<ObjectResult> DeleteUserAsync(string userId);
    }
}