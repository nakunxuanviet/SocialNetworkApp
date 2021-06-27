using SocialNetwork.Domain.Entities.Accounts;
using SocialNetwork.Domain.Entities.ApplicationUsers;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string CreateToken(ApplicationUser user);

        RefreshToken GenerateRefreshToken();

        string CreateWithRoles(ApplicationUser user);

        bool IsValidResetPasswordToken(string token);

        string CreateResetPasswordToken();
    }
}