using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Common.Interfaces
{
    public interface ILoggerManager
    {
        /// <summary>
        /// Log information.
        /// </summary>
        /// <param name="message"></param>
        void LogInfo(string message);

        /// <summary>
        /// Log warning.
        /// </summary>
        /// <param name="message"></param>
        void LogWarn(string message);

        /// <summary>
        /// Log debug.
        /// </summary>
        /// <param name="message"></param>
        void LogDebug(string message);

        /// <summary>
        /// Log error.
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message);
    }
}