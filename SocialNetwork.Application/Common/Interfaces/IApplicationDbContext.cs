using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities.Activities;
using SocialNetwork.Domain.Entities.Settings;
using SocialNetwork.Domain.Entities.TodoItems;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
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
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}