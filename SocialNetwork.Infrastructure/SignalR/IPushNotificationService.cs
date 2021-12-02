using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.SignalR
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPushNotificationService
    {
        Task SendAsync(PushNotification notification);
    }
}