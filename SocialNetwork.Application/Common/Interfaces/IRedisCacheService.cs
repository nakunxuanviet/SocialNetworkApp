using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IRedisCacheService
    {
        T Get<T>(string key);

        T Set<T>(string key, T value);

        Task<T> GetAsync<T>(string key);

        Task<T> SetAsync<T>(string key, T value);
    }
}