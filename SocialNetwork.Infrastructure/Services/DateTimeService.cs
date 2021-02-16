using SocialNetwork.Application.Common.Interfaces;
using System;

namespace SocialNetwork.Infrastructure.Services
{
    public class DateTimeService : IDateTime
    {
        public DateTime Now => DateTime.Now;
    }
}