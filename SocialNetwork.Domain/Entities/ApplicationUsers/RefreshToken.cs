using SocialNetwork.Domain.Entities.ApplicationUsers;
using System;

namespace SocialNetwork.Domain.Entities.Accounts
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public ApplicationUser User { get; set; }
        public String UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;

        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        public string RevokedByIp { get; set; }
        //public string ReplacedByToken { get; set; }
    }
}