using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.Entities.ApplicationRoles;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfigurations.PermissionEntityConfiguration
{
    public class MenuItemEntityTypeConfig : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> builder)
        {
            builder.ToTable("MenuItems");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Title).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(100);
            builder.Property(x => x.Icon).HasMaxLength(50);
            builder.Property(x => x.Url).HasMaxLength(50);

            builder.HasMany(y => y.Children)
                .WithOne(r => r.ParentItem)
                .HasForeignKey(u => u.ParentId);

            builder.HasMany(t => t.RoleMenus)
                .WithOne(u => u.MenuItem)
                .HasForeignKey(r => r.MenuId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
