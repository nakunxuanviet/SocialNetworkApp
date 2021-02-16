using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.SeedWork;

namespace SocialNetwork.Infrastructure.Persistence.Configuration
{
    /// <summary>
    /// Common Property Configuration
    /// </summary>
    public abstract class CommonPropertyConfiguration
    {
        protected static void ConfigureBase<TEntity>(EntityTypeBuilder<TEntity> builder) where TEntity : class, IAuditableEntity, IDeleteEntity
        {
            builder.Property(e => e.IsDeleted).IsRequired().HasDefaultValue(0);
            builder.Property(e => e.CreatedAt);
            builder.Property(e => e.CreatedBy).HasMaxLength(36).IsFixedLength().IsUnicode(false);
            builder.Property(e => e.UpdatedAt);
            builder.Property(e => e.UpdatedBy).HasMaxLength(36).IsFixedLength().IsUnicode(false);
        }
    }
}