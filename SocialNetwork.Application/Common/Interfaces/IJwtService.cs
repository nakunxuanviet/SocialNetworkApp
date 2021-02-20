using SocialNetwork.Domain.Entities.Accounts;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string CreateToken(AppUser user);

        RefreshToken GenerateRefreshToken();

        string CreateWithRoles(AppUser user);

        bool IsValidResetPasswordToken(string token);

        string CreateResetPasswordToken();
    }
}