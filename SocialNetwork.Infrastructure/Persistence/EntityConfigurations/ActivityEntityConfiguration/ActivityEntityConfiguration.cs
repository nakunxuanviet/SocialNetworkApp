using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialNetwork.Domain.Entities.Activities;

namespace SocialNetwork.Infrastructure.Persistence.EntityConfigurations.ActivityEntityConfiguration
{
    public class ActivityEntityConfiguration : CommonPropertyConfiguration, IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.ToTable("Activities");
            builder.HasKey(o => o.Id);
            builder.Ignore(b => b.DomainEvents);

            builder.Property(o => o.Title).HasMaxLength(50).IsRequired();
            builder.Property(o => o.Date).IsRequired();
            builder.Property(x => x.Category).HasMaxLength(50);
            builder.Property(x => x.City).HasMaxLength(50);
            builder.Property(x => x.Description).HasMaxLength(150);
            builder.Property(x => x.Venue).IsRequired();
            builder.Property(x => x.IsCancelled).HasDefaultValue(false);

            //ConfigureBase(builder);

            builder.HasQueryFilter(o => o.IsDeleted == false);
        }
    }
}