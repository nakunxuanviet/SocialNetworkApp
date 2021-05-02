namespace SocialNetwork.Application.Common.Interfaces
{
    public interface ICacheService
    {
        //T Get<T>(string key);

        bool TryGet<T>(string cacheKey, out T value);

        T Set<T>(string cacheKey, T value);

        void Remove(string cacheKey);
    }
}