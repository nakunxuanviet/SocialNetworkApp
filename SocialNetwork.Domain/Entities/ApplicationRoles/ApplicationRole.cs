using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SocialNetwork.Domain.Entities.ApplicationRoles
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() { }
        public ApplicationRole(string name)
            : this()
        {
            Name = name;
            RoleMenus = new HashSet<ApplicationRoleMenu>();
        }

        public ICollection<ApplicationRoleMenu> RoleMenus { get; set; }
    }
}
