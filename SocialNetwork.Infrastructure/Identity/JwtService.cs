using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Application.Accounts.Models;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Domain.Entities.Accounts;

namespace SocialNetwork.Infrastructure.Identity
{
    public class JwtService : IJwtService
    {
        private readonly JwtOptions _options;
        private readonly JwtResetPasswordOptions _rsOptions;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public JwtService(IOptions<JwtOptions> options, IOptions<JwtResetPasswordOptions> rsOptions)
        {
            _options = options.Value;
            _rsOptions = rsOptions.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public string CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Audience = _options.Audience,
                Issuer = _options.Issuer,
                Expires = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes),
                SigningCredentials = creds
            };

            //var tokenHandler = new JwtSecurityTokenHandler();

            var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);

            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        public RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return new RefreshToken { Token = Convert.ToBase64String(randomNumber) };
        }

        public string CreateWithRoles(ApplicationUser user)
        {
            var currentTime = DateTime.UtcNow;
            var expiredTime = DateTime.UtcNow.AddMinutes(_options.ExpiryMinutes);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("IsAdmin", user.IsAdmin.ToString()),
            };

            //foreach (var role in user.roles)
            //{
            //    claims.Add(new Claim(ClaimTypes.Role, role));
            //}

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var jwt = new JwtSecurityToken(_options.Issuer,
                _options.Audience,
                claims,
                currentTime,
                expiredTime,
                creds);

            return _jwtSecurityTokenHandler.WriteToken(jwt);
        }

        public bool IsValidResetPasswordToken(string token)
        {
            try
            {
                _jwtSecurityTokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_rsOptions.SecretKey)),

                    ValidateIssuer = true,
                    ValidIssuer = _rsOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = _rsOptions.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out var _);

                return true;
            }
            catch (Exception)
            {
                // Validate Token fail
                return false;
            }
        }

        public string CreateResetPasswordToken()
        {
            var currentTime = DateTime.UtcNow;
            var expiredTime = DateTime.UtcNow.AddMinutes(_rsOptions.ExpiryMinutes);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_rsOptions.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var jwt = new JwtSecurityToken(_rsOptions.Issuer,
                _rsOptions.Audience,
                null,
                currentTime,
                expiredTime,
                creds);

            return _jwtSecurityTokenHandler.WriteToken(jwt);
        }
    }
}