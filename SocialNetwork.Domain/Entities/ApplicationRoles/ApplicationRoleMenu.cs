using System.Collections.Generic;

namespace SocialNetwork.Domain.Entities.ApplicationRoles
{
    public class ApplicationRoleMenu
    {
        public ApplicationRoleMenu()
        {
            Permissions = new HashSet<MenuPermission>();
        }

        public virtual int Id { get; set; }

        public virtual string RoleId { get; set; }

        public virtual int MenuId { get; set; }

        public virtual ApplicationRole Role { get; set; }
        public virtual MenuItem MenuItem { get; set; }
        public ICollection<MenuPermission> Permissions { get; set; }
    }
}
