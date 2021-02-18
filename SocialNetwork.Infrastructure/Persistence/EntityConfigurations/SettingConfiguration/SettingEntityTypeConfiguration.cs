using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.Entities.Settings;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfigurations.SettingConfiguration
{
    internal class SettingEntityTypeConfiguration : CommonPropertyConfiguration, IEntityTypeConfiguration<Setting>
    {
        public void Configure(EntityTypeBuilder<Setting> builder)
        {
            builder.ToTable("Settings");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.Key).HasMaxLength(50).IsRequired();
            builder.Property(o => o.Value).IsRequired();

            builder.HasIndex(x => x.Key).IsUnique();

            //ConfigureBase(builder);

            builder.HasQueryFilter(o => o.IsDeleted == false);
        }
    }
}