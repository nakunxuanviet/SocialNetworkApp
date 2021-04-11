using System;

namespace SocialNetwork.Application.Common.Exceptions
{
    /// <summary>
    /// Exception type for Forbid exceptions
    /// </summary>
    public class ForbidException : Exception
    {
        public ForbidException()
        { }

        public ForbidException(string message)
            : base(message)
        { }

        public ForbidException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}