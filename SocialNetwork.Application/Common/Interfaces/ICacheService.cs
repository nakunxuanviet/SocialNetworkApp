namespace SocialNetwork.Application.Common.Interfaces
{
    public interface ICacheService
    {
        T Get<T>(string key);

        T Set<T>(string key, T value);
    }
}