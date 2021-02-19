using Microsoft.AspNetCore.Identity;
using SocialNetwork.Domain.Entities;
using SocialNetwork.Domain.Entities.Activities;
using SocialNetwork.Infrastructure.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "Administrator1!");
            }
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
            if (!context.Activities.Any())
            {
                context.Activities.Add(new Activity
                {
                    Title = "Shopping",
                    Date = DateTime.Now,
                    Description = "Lorem lorem lorem",
                    Category = "Sport",
                    City = "Hue",
                    Venue = "20",
                    IsCancelled = false
                });

                await context.SaveChangesAsync();
            }
        }
    }
}