using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Application.Accounts.Models
{
    public class LogoutRequest
    {
        [Required]
        public string Email { get; set; }
    }
}