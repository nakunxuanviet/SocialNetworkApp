using Microsoft.AspNetCore.Identity;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Domain.Entities.ApplicationRoles;
using SocialNetwork.Domain.Entities.ApplicationUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Menu
{
    /// <summary>
    /// To create your own permission, add an entry to the Permissions table
    /// and call GetMenuByUser() by passing in your own permission.
    /// </summary>
    public class MenuManager
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public MenuManager(IApplicationDbContext context, UserManager<ApplicationUser> manager, RoleManager<ApplicationRole> roleManager)
        {
            _context = context;
            _userManager = manager;
            _roleManager = roleManager;
        }

        public async Task<List<MenuPermission>> GetMenuPermissionsByUser(ApplicationUser user)
        {
            if (user == null) return new List<MenuPermission>();

            var role = await _roleManager.FindByIdAsync(user.Id);

            return role != null ? role.RoleMenus.SelectMany(x => x.Permissions).ToList() : new List<MenuPermission>();

            //return _context.Roles
            //    .Include(role => role.MenuItems.Select(menu => menu.Permissions))
            //    .Where(role => role.Users.Any(tableUser => tableUser.UserId == user.Id))
            //    .SelectMany(role => role.MenuItems.SelectMany(roleMenu => roleMenu.Permissions))
            //    .ToList();
        }


        public async Task<List<MenuItem>> GetMenuByUser(ApplicationUser user, Func<MenuPermission, bool> filterFunc = null)
        {
            var items = await GetMenuPermissionsByUser(user);

            var records = filterFunc == null
                ? items.ToList()
                : items.Where(filterFunc).ToList();

            return records
                .GroupBy(menuPermission => menuPermission.RoleMenu.MenuItem)
                .Select(grouping => grouping.Key)
                .ToList();
        }

        public List<MenuItem> GetMenuByUser(IPrincipal user)
        {
            if (user == null)
            {
                return new List<MenuItem>();
            }

            var principal = user as ClaimsPrincipal;

            var id = _userManager.GetUserId(principal);

            var viewableItems = principal.Claims
                .Where(e => e.Value == "View")
                .Select(item => item.Type)
                .ToList();

            var result = _context.MenuItems
                .Where(item => viewableItems.Any(u => item.Id.ToString() == u))
                .ToList();

            return result;
        }
    }
}
