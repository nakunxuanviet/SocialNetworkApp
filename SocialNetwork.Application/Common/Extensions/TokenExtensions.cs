using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace SocialNetwork.Application.Common.Extensions
{
    public static class TokenExtensions
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Is Admin
        /// </summary>
        /// <returns></returns>
        public static bool IsAdmin()
        {
            var claims = _httpContextAccessor.HttpContext.GetClaims();
            return true.ToString() == claims.FirstOrDefault(c => c.Type == "IsAdmin")?.Value;
        }

        /// <summary>
        /// Get UserId
        /// </summary>
        /// <returns></returns>
        public static string GetUserId()
        {
            var claims = _httpContextAccessor.HttpContext.GetClaims();
            return claims?.FirstOrDefault(c => c.Type == "UserId")?.Value;
        }

        /// <summary>
        /// Get Privileges
        /// </summary>
        /// <returns></returns>
        public static List<string> GetPrivileges()
        {
            var claims = _httpContextAccessor.HttpContext.GetClaims();
            var privileges = claims?.FirstOrDefault(c => c.Type == "Privileges")?.Value;
            return privileges?.Split(",").ToList() ?? new List<string>();
        }

        /// <summary>
        /// Get Organizations
        /// </summary>
        /// <returns></returns>
        public static List<string> GetOrganizations()
        {
            var claims = _httpContextAccessor.HttpContext.GetClaims();
            var organizations = claims?.FirstOrDefault(c => c.Type == "Organizations")?.Value;
            return organizations?.Split(",").ToList() ?? new List<string>();
        }

        /// <summary>
        /// Get TimezoneOffset
        /// </summary>
        /// <returns></returns>
        public static int GetTimezoneOffset()
        {
            var claims = _httpContextAccessor.HttpContext.GetClaims();
            var value = claims?.FirstOrDefault(c => c.Type == "TimezoneOffset")?.Value;
            return int.TryParse(value, out var result) ? result : 0;
        }

        #region Private method
        private static List<Claim> GetClaims(this HttpContext httpContext)
        {
            var token = httpContext.GetToken();
            if (token == null)
            {
                return new List<Claim>();
            }
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            try
            {
                return jwtSecurityTokenHandler.ReadJwtToken(token).Claims.ToList();
            }
            catch
            {
                return new List<Claim>();
            }
        }

        private static string GetToken(this HttpContext httpContext)
        {
            var header = httpContext.Request.Headers;
            var hasToken = header.TryGetValue("accessToken", out var accessToken);
            if (!hasToken) return null;
            return accessToken;
        }
        #endregion
    }
}
