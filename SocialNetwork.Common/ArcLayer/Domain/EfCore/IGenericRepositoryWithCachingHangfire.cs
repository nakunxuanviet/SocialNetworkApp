using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.Common.ArcLayer.Domain.EfCore
{
    public interface IGenericRepositoryWithCachingHangfire<T> where T : class
    {
        Task<T> GetByIdAsync(int id);

        Task<IReadOnlyList<T>> GetAllAsync();

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}