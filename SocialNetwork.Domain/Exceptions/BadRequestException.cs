using System;

namespace SocialNetwork.Domain.Exceptions
{
    /// <summary>
    /// Exception type for not found exceptions
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