using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using SocialNetwork.Infrastructure.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SocialNetwork.Infrastructure.SignalR
{
    public class PushNotificationService : IPushNotificationService
    {
        private readonly IHubContext<PushNotificationHub, IClientPushNotification> _pushNotificationHubContext;

        public PushNotificationService(IHubContext<PushNotificationHub, IClientPushNotification> pushNotificationHubContext, IOptions<PushNotificationOptions> options)
        {
            _pushNotificationHubContext = pushNotificationHubContext;

        }

        public async Task SendAsync(PushNotification notification)
        {
            //sent to one or more connectionId
            if (!string.IsNullOrEmpty(notification.ReciversConnectionIds))
            {
                var connectionIds = notification.ReciversConnectionIds.Split(',');

                await _pushNotificationHubContext.Clients.Clients(connectionIds).Recive(notification);
                return;
            }


            // send to all clients except caller
            if (!string.IsNullOrEmpty(notification.CallerConnectionId))
            {
                await _pushNotificationHubContext.Clients.AllExcept(notification.CallerConnectionId).Recive(notification);
                return;
            }
        }
    }
}
