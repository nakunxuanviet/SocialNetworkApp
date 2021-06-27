namespace SocialNetwork.Domain.Entities.ApplicationRoles
{
    public class MenuPermission
    {
        public virtual int RoleMenuId { get; set; }

        public virtual int PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual ApplicationRoleMenu RoleMenu { get; set; }
    }
}
