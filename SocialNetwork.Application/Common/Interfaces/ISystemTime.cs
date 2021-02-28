using System;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface ISystemTime
    {
        /// <summary>
        /// Get curent date time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Get curent date time UTC.
        /// </summary>
        DateTime UtcNow { get; }
    }
}