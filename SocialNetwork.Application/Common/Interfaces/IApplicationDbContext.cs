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

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}