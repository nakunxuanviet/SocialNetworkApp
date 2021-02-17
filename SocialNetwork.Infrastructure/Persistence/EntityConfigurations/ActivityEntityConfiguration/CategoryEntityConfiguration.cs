//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using SocialNetwork.Domain.Entities.Activities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SocialNetwork.Infrastructure.Persistence.EntityConfigurations.ActivityEntityConfiguration
//{
//    public class CategoryEntityConfiguration : CommonPropertyConfiguration, IEntityTypeConfiguration<Category>
//    {
//        public void Configure(EntityTypeBuilder<Category> builder)
//        {
//            builder.ToTable("Categories");
//            builder.HasKey(o => o.Id);
//            builder.Property(o => o.Name).HasMaxLength(20).IsRequired();

//            builder.HasQueryFilter(o => o.IsDeleted == false);

//            builder.HasOne(a => a.Activity).WithOne(c => c.Category).HasForeignKey<Activity>(fk => fk.CategoryId);
//        }
//    }
//}