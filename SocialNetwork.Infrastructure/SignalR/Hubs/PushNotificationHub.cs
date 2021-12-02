using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.SignalR.Hubs
{
    public class PushNotificationHub : Hub<IClientPushNotification>
    {

        public override Task OnConnectedAsync()
        {
            Clients.Caller.Connect(Context.ConnectionId);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
