using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.SeedWork;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfigurations
{
    public abstract class EnumEntityTypeConfiguration
    {
        protected static void ConfigureBase<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : Enumeration
        {
            builder.Property(o => o.Id)
                .HasDefaultValue(1)
                .ValueGeneratedNever()
                .IsRequired();

            builder.Property(o => o.Name)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
