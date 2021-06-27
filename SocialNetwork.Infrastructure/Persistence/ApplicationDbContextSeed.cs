using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities.Activities;
using SocialNetwork.Domain.Entities.ApplicationRoles;
using SocialNetwork.Domain.Entities.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {
            var defaultUser = new ApplicationUser { UserName = "admin", Email = "admin@gmail.com", DisplayName = "Administrator", IsAdmin = true, EmailConfirmed = true };

            if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
            {
                await userManager.CreateAsync(defaultUser, "Admin@1234");
            }
        }

        public static async Task SeedUserAndRolePermissionAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext dbContext)
        {
            var adminRoleId = "79AC1299-E87C-446C-AD69-ECDFE1A5EEA1";
            var userRoleId = "7AAFEC08-0A31-4588-A290-62B12F888920";

            if (!await dbContext.Permissions.AnyAsync())
            {
                await dbContext.Permissions.AddRangeAsync(new List<Permission>
                {
                    new Permission {Name = "View"},
                    new Permission {Name = "Create"},
                    new Permission {Name = "Update"},
                    new Permission {Name = "Delete"},
                    new Permission {Name = "Upload"},
                    new Permission {Name = "Publish"}
                });
                await dbContext.SaveChangesAsync();
            }

            if (!await dbContext.MenuItems.AnyAsync())
            {
                var setupItem = await dbContext.MenuItems.AddAsync(new MenuItem { Title = "Setup", ParentId = null, Icon = "wrench", Url = null });
                await dbContext.SaveChangesAsync();

                await dbContext.MenuItems.AddRangeAsync(new List<MenuItem>
                {
                    new MenuItem {Title = "Users", ParentId = setupItem.Entity.Id, Icon = "user", Url = "/Setting/Users"},
                    new MenuItem {Title = "Security", ParentId = setupItem.Entity.Id, Icon = "lock", Url = "/Setting/Security"},
                    new MenuItem {Title = "Menu Management", ParentId = setupItem.Entity.Id, Icon = "menu", Url = "/Setting/Menu"}
                });
                await dbContext.SaveChangesAsync();
            }

            if (!await roleManager.Roles.AnyAsync())
            {
                await roleManager.CreateAsync(new ApplicationRole { Id = adminRoleId, Name = "Administrator" });
                await roleManager.CreateAsync(new ApplicationRole { Id = userRoleId, Name = "User" });
            }

            if (!await dbContext.RoleMenus.AnyAsync())
            {
                await dbContext.RoleMenus.AddRangeAsync(new List<ApplicationRoleMenu>
                {
                    // Admin
                    new ApplicationRoleMenu {RoleId=adminRoleId, MenuId=1},
                    new ApplicationRoleMenu {RoleId=adminRoleId, MenuId=2},
                    new ApplicationRoleMenu {RoleId=adminRoleId, MenuId=3},
                    new ApplicationRoleMenu {RoleId=adminRoleId, MenuId=4},
                    // User
                    new ApplicationRoleMenu {RoleId=userRoleId, MenuId=1},
                    new ApplicationRoleMenu {RoleId=userRoleId, MenuId=2},

                });
                await dbContext.SaveChangesAsync();
            }

            if (!await dbContext.MenuPermissions.AnyAsync())
            {
                List<MenuPermission> menuPermissions = new List<MenuPermission>();
                var roleMenus = await dbContext.RoleMenus.ToListAsync();

                roleMenus.ForEach(x =>
                {
                    // View Permission for Role Admin
                    if (x.RoleId.Equals(adminRoleId) && x.MenuId == 1) menuPermissions.Add(new MenuPermission { RoleMenuId = x.Id, PermissionId = 1 });
                    if (x.RoleId.Equals(adminRoleId) && x.MenuId == 2) menuPermissions.Add(new MenuPermission { RoleMenuId = x.Id, PermissionId = 1 });
                    if (x.RoleId.Equals(adminRoleId) && x.MenuId == 3) menuPermissions.Add(new MenuPermission { RoleMenuId = x.Id, PermissionId = 1 });
                    if (x.RoleId.Equals(adminRoleId) && x.MenuId == 4) menuPermissions.Add(new MenuPermission { RoleMenuId = x.Id, PermissionId = 1 });

                    // View Permission for Role User
                    if (x.RoleId.Equals(userRoleId) && x.MenuId == 1) menuPermissions.Add(new MenuPermission { RoleMenuId = x.Id, PermissionId = 1 });
                    if (x.RoleId.Equals(userRoleId) && x.MenuId == 2) menuPermissions.Add(new MenuPermission { RoleMenuId = x.Id, PermissionId = 1 });
                });
                await dbContext.MenuPermissions.AddRangeAsync(menuPermissions);
                await dbContext.SaveChangesAsync();
            }


            var adminId = "C316633F-7DE3-48BE-A117-807AF8F7BE70";
            var userId = "102EBBD8-EFDA-462C-B7C2-1F5D88C80456";

            // Create Admin
            ApplicationUser admin = await userManager.FindByEmailAsync("admin@gmail.com");
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    Id = adminId,
                    Email = "admin@gmail.com",
                    UserName = "admin@gmail.com",
                    DisplayName = "Admin",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    NormalizedUserName = "ADMIN",
                    NormalizedEmail = "admin@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await userManager.CreateAsync(admin, "Admin@1234");
                await userManager.AddToRoleAsync(admin, "Administrator");
            }

            // Create User
            ApplicationUser user = await userManager.FindByEmailAsync("viet.tx@gmail.com");
            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = userId,
                    Email = "viet.tx@gmail.com",
                    UserName = "viet.tx@gmail.com",
                    DisplayName = "Tran Xuan Viet",
                    EmailConfirmed = true,
                    NormalizedUserName = "VIET.TX",
                    NormalizedEmail = "VIET.TX@GMAIL.COM",
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                await userManager.CreateAsync(user, "Admin@1234");
                await userManager.AddToRoleAsync(user, "User");
            }

            // If you want this by user, add the claims for each user.
            var adminClaims = await userManager.GetClaimsAsync(admin);
            if (adminClaims.Count(y => y.Value == "View") == 0)
            {
                await userManager.AddClaimsAsync(admin, new List<Claim>
                {
                    // Claim(ClaimType, ClaimValue)
                    // ClaimType: MenuItemId
                    // ClaimValue: Permission name
                    new Claim(1.ToString(), "View"),
                    new Claim(2.ToString(), "View"),
                    new Claim(3.ToString(), "View"),
                    new Claim(4.ToString(), "View")
                });
            }

            var userClaims = await userManager.GetClaimsAsync(user);
            if (userClaims.Count(y => y.Value == "View") == 0)
            {
                await userManager.AddClaimsAsync(user, new List<Claim>
                {
                    new Claim(1.ToString(), "View"),
                    new Claim(2.ToString(), "View")
                });

            }
        }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            if (!context.Activities.Any())
            {
                var activities = new List<Activity>
                {
                    new Activity
                    {
                        Title = "Past Activity 1",
                        Date = DateTime.Now.AddMonths(-2),
                        Description = "Activity 2 months ago",
                        Category = "drinks",
                        City = "London",
                        Venue = "Pub",
                        //Attendees = new List<ActivityAttendee>
                        //{
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[0],
                        //        IsHost = true
                        //    }
                        //}
                    },
                    new Activity
                    {
                        Title = "Past Activity 2",
                        Date = DateTime.Now.AddMonths(-1),
                        Description = "Activity 1 month ago",
                        Category = "culture",
                        City = "Paris",
                        Venue = "The Louvre",
                        //Attendees = new List<ActivityAttendee>
                        //{
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[0],
                        //        IsHost = true
                        //    },
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[1],
                        //        IsHost = false
                        //    },
                        //}
                    },
                    new Activity
                    {
                        Title = "Future Activity 1",
                        Date = DateTime.Now.AddMonths(1),
                        Description = "Activity 1 month in future",
                        Category = "music",
                        City = "London",
                        Venue = "Wembly Stadium",
                        //Attendees = new List<ActivityAttendee>
                        //{
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[2],
                        //        IsHost = true
                        //    },
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[1],
                        //        IsHost = false
                        //    },
                        //}
                    },
                    new Activity
                    {
                        Title = "Future Activity 2",
                        Date = DateTime.Now.AddMonths(2),
                        Description = "Activity 2 months in future",
                        Category = "food",
                        City = "London",
                        Venue = "Jamies Italian",
                        //Attendees = new List<ActivityAttendee>
                        //{
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[0],
                        //        IsHost = true
                        //    },
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[2],
                        //        IsHost = false
                        //    },
                        //}
                    },
                    new Activity
                    {
                        Title = "Future Activity 3",
                        Date = DateTime.Now.AddMonths(3),
                        Description = "Activity 3 months in future",
                        Category = "drinks",
                        City = "London",
                        Venue = "Pub",
                        //Attendees = new List<ActivityAttendee>
                        //{
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[1],
                        //        IsHost = true
                        //    },
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[0],
                        //        IsHost = false
                        //    },
                        //}
                    },
                    new Activity
                    {
                        Title = "Future Activity 4",
                        Date = DateTime.Now.AddMonths(4),
                        Description = "Activity 4 months in future",
                        Category = "culture",
                        City = "London",
                        Venue = "British Museum",
                        //Attendees = new List<ActivityAttendee>
                        //{
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[1],
                        //        IsHost = true
                        //    }
                        //}
                    },
                    new Activity
                    {
                        Title = "Future Activity 5",
                        Date = DateTime.Now.AddMonths(5),
                        Description = "Activity 5 months in future",
                        Category = "drinks",
                        City = "London",
                        Venue = "Punch and Judy",
                        //Attendees = new List<ActivityAttendee>
                        //{
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[0],
                        //        IsHost = true
                        //    },
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[1],
                        //        IsHost = false
                        //    },
                        //}
                    },
                    new Activity
                    {
                        Title = "Future Activity 6",
                        Date = DateTime.Now.AddMonths(6),
                        Description = "Activity 6 months in future",
                        Category = "music",
                        City = "London",
                        Venue = "O2 Arena",
                        //Attendees = new List<ActivityAttendee>
                        //{
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[2],
                        //        IsHost = true
                        //    },
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[1],
                        //        IsHost = false
                        //    },
                        //}
                    },
                    new Activity
                    {
                        Title = "Future Activity 7",
                        Date = DateTime.Now.AddMonths(7),
                        Description = "Activity 7 months in future",
                        Category = "travel",
                        City = "Berlin",
                        Venue = "All",
                        //Attendees = new List<ActivityAttendee>
                        //{
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[0],
                        //        IsHost = true
                        //    },
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[2],
                        //        IsHost = false
                        //    },
                        //}
                    },
                    new Activity
                    {
                        Title = "Future Activity 8",
                        Date = DateTime.Now.AddMonths(8),
                        Description = "Activity 8 months in future",
                        Category = "drinks",
                        City = "London",
                        Venue = "Pub", 
                        //Attendees = new List<ActivityAttendee>
                        //{
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[2],
                        //        IsHost = true
                        //    },
                        //    new ActivityAttendee
                        //    {
                        //        ApplicationUser = users[1],
                        //        IsHost = false
                        //    },
                        //}
                    }
                };

                await context.Activities.AddRangeAsync(activities);
                await context.SaveChangesAsync();
            }
        }
    }
}