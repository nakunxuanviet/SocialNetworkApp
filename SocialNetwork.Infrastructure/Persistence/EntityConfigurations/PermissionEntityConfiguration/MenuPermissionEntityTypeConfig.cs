using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.Entities.ApplicationRoles;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfigurations.PermissionEntityConfiguration
{
    public class MenuPermissionEntityTypeConfig : IEntityTypeConfiguration<MenuPermission>
    {
        public void Configure(EntityTypeBuilder<MenuPermission> builder)
        {
            builder.ToTable("MenuPermissions");
            builder.HasKey(l => new { l.RoleMenuId, l.PermissionId });

            builder.HasOne(o => o.Permission)
                .WithMany(i => i.MenuPermissions)
                .IsRequired();

            builder.HasOne(o => o.RoleMenu)
                .WithMany(i => i.Permissions)
                .IsRequired();
        }
    }
}
