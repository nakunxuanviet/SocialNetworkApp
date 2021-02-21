using SocialNetwork.Domain.Entities.Accounts;

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