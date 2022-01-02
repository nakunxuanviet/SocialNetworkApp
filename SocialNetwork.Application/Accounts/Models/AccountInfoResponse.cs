using System.Text.Json.Serialization;

namespace SocialNetwork.Application.Accounts.Models
{
    public class AccountInfoResponse
    {
        public string DisplayName { get; set; }
        public string Username { get; set; }
        public string Image { get; set; }
        public string AccessToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }
    }
}