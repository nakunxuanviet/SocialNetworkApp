using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Application.Accounts.Models
{
    public class JwtOptions
    {
        /// <summary>
        /// Secret Key
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Expiry Minutes
        /// </summary>
        public int ExpiryMinutes { get; set; }

        /// <summary>
        /// Issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Audience
        /// </summary>
        public string Audience { get; set; }
    }
}