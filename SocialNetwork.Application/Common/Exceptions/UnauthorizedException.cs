using System;

namespace SocialNetwork.Application.Common.Exceptions
{
    /// <summary>
    /// Exception type for Unauthorized exceptions
    /// </summary>
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        { }

        public UnauthorizedException(string message)
            : base(message)
        { }

        public UnauthorizedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}