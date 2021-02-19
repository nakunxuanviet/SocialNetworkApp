using System;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface IDateTime
    {
        /// <summary>
        /// Get curent date time.
        /// </summary>
        DateTime Now { get; }
    }
}