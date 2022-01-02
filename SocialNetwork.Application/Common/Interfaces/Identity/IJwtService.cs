using SocialNetwork.Domain.Entities.Accounts;
using SocialNetwork.Domain.Entities.ApplicationUsers;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string CreateJwtToken(ApplicationUser user);

        Task<RefreshToken> GenerateRefreshToken(ApplicationUser user, string ipAddress);

        Task<bool> RevokeToken(string token, string ipAddress);

        string CreateWithRoles(ApplicationUser user);

        bool IsValidResetPasswordToken(string token);

        string CreateResetPasswordToken();
    }
}