using System.Collections.Generic;

namespace SocialNetwork.Domain.Entities.ApplicationRoles
{
    public class Permission
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<MenuPermission> MenuPermissions { get; set; }
    }
}
