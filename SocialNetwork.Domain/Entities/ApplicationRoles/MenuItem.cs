using System.Collections.Generic;

namespace SocialNetwork.Domain.Entities.ApplicationRoles
{
    public class MenuItem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MenuItem()
        {
            Children = new HashSet<MenuItem>();
            RoleMenus = new HashSet<ApplicationRoleMenu>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public int? ParentId { get; set; }

        public string Icon { get; set; }

        public string Url { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MenuItem> Children { get; set; }

        public virtual MenuItem ParentItem { get; set; }

        public virtual ICollection<ApplicationRoleMenu> RoleMenus { get; set; }
    }
}
