using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.SeedWork;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Domain.Entities.Accounts
{
    public class ApplicationUser : IdentityUser, IAuditableEntity, IDeleteEntity
    {
        public string DisplayName { get; set; }
        public string Bio { get; set; }
        public bool IsAdmin { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}