namespace SocialNetwork.Application.Common.Interfaces
{
    /// <summary>
    /// This type eliminates the need to depend directly on the ASP.NET Core logging types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ILoggerManager<T>
    {
        /// <summary>
        /// Log information.
        /// </summary>
        /// <param name="message"></param>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// Log warning.
        /// </summary>
        /// <param name="message"></param>
        void LogWarn(string message, params object[] args);

        /// <summary>
        /// Log debug.
        /// </summary>
        /// <param name="message"></param>
        void LogDebug(string message, params object[] args);

        /// <summary>
        /// Log error.
        /// </summary>
        /// <param name="message"></param>
        void LogError(string message, params object[] args);
    }
}