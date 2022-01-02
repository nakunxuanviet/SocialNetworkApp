using System.ComponentModel.DataAnnotations;

namespace SocialNetwork.Application.Accounts.Models
{
    public class ResetPasswordRequest
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}