//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using SocialNetwork.Domain.Entities.Activities;

//namespace SocialNetwork.Infrastructure.Persistence.EntityConfigurations.ActivityEntityConfiguration
//{
//    public class CityEntityConfiguration : CommonPropertyConfiguration, IEntityTypeConfiguration<City>
//    {
//        public void Configure(EntityTypeBuilder<City> builder)
//        {
//            builder.ToTable("Cities");
//            builder.HasKey(o => o.Id);
//            builder.Property(o => o.Name).HasMaxLength(20).IsRequired();

//            builder.HasQueryFilter(o => o.IsDeleted == false);
//            builder.HasOne(a => a.Activity).WithOne(c => c.City).HasForeignKey<Activity>(c => c.CityId);
//        }
//    }
//}