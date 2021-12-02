using SocialNetwork.Infrastructure.SignalR;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.SignalR.Hubs
{
    public interface IClientPushNotification
    {
        Task Recive(PushNotification notification);
        Task Connect(string connectionId);
    }
}
