using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SocialNetwork.Infrastructure.SignalR;

namespace SocialNetwork.Infrastructure.SignalR.Sample
{
    public class HomeController : Controller
    {

        private readonly IPushNotificationService _pushNotificationService;
        public HomeController(IPushNotificationService pushNotificationService)
        {
            _pushNotificationService = pushNotificationService;
        }

        public IActionResult Index()
        {
            return View(new PushNotification());
        }

        [HttpPost]
        public async Task SendNotification(PushNotification notification)
        {
            await _pushNotificationService.SendAsync(notification);
        }
    }
}
