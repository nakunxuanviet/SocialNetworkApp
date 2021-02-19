using System;

namespace SocialNetwork.Application.Common.Exceptions
{
    /// <summary>
    /// Exception type for bad request exceptions
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException()
        { }

        public BadRequestException(string message)
            : base(message)
        { }

        public BadRequestException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}