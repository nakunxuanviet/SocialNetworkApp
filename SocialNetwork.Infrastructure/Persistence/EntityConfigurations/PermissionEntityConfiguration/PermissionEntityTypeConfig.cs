using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.Entities.ApplicationRoles;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfigurations.PermissionEntityConfiguration
{
    public class PermissionEntityTypeConfig : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable("Permission");

            builder.HasKey(o => o.Id);
            builder.Property(o => o.Name).HasMaxLength(50).IsRequired();

            builder.HasMany(o => o.MenuPermissions)
                .WithOne(i => i.Permission)
                .HasForeignKey(y => y.PermissionId);
        }
    }
}
