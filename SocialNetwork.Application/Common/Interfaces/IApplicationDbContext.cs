using Microsoft.EntityFrameworkCore;
using SocialNetwork.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Activity> Activities { get; set; }
        DbSet<TodoItem> TodoItems { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}