using System;

namespace SocialNetwork.Application.Common.Exceptions
{
    /// <summary>
    /// Exception type for validate exceptions
    /// </summary>
    public class IdentityValidationException : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public IdentityValidationException()
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        public IdentityValidationException(string message)
            : base(message)
        { }

        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public IdentityValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}