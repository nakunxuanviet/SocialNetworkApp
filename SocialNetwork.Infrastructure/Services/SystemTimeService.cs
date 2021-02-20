using SocialNetwork.Application.Common.Interfaces;
using System;

namespace SocialNetwork.Infrastructure.Services
{
    public class SystemTimeService : ISystemTime
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}