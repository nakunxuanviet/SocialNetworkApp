using System;

namespace SocialNetwork.Domain.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Convert to date time default
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static string ToFullDateTime(this DateTime datetime)
        {
            return datetime.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}