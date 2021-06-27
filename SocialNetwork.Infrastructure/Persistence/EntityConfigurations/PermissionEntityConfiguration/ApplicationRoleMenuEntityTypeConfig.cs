using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.Entities.ApplicationRoles;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfigurations.PermissionEntityConfiguration
{
    public class ApplicationRoleMenuEntityTypeConfig : IEntityTypeConfiguration<ApplicationRoleMenu>
    {
        public void Configure(EntityTypeBuilder<ApplicationRoleMenu> builder)
        {
            builder.ToTable("RoleMenus");
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Role)
                .WithMany(u => u.RoleMenus)
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.MenuItem)
                .WithMany(u => u.RoleMenus)
                .HasForeignKey(e => e.MenuId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
