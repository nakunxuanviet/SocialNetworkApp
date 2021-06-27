﻿using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities.Activities;
using SocialNetwork.Domain.Entities.ApplicationRoles;
using SocialNetwork.Domain.Entities.AuditLog;
using SocialNetwork.Domain.Entities.Settings;
using SocialNetwork.Domain.Entities.TodoItems;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuPermission> MenuPermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<ApplicationRoleMenu> RoleMenus { get; set; }

        DbSet<Audit> AuditLogs { get; set; }
        DbSet<Setting> Settings { get; set; }
        DbSet<Activity> Activities { get; set; }
        DbSet<TodoItem> TodoItems { get; set; }

        //DbSet<Category> Categories { get; set; }
        //DbSet<City> Cities { get; set; }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}