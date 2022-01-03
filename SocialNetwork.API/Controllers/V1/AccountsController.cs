﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SocialNetwork.Application.Accounts.Models;
using SocialNetwork.Application.Common.Exceptions;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Emails;
using SocialNetwork.Domain.Entities.ApplicationUsers;
using SocialNetwork.Domain.Shared;

namespace SocialNetwork.API.Controllers.V1
{
    [ApiVersion("1.0")]
    public class AccountsController : BaseApiController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _tokenService;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        private readonly ISendGridEmailService _sendGridEmailSender;
        private readonly ISmtpMailService _emailService;

        public AccountsController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IJwtService tokenService,
            IConfiguration config, ISendGridEmailService sendGridEmailSender, ISmtpMailService emailService)
        {
            _sendGridEmailSender = sendGridEmailSender;
            _config = config;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://graph.facebook.com")
            };
            _emailService = emailService;
        }

        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AccountInfoResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.Users
                //.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Email == request.Email);

            if (user == null) return Unauthorized("Invalid email");

            if (user.UserName == "admin") user.EmailConfirmed = true;

            if (!user.EmailConfirmed) return Unauthorized("Email not confirmed");

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid password");
            }

            var userResult = await SetRefreshToken(user);

            // Save token
            await _userManager.SetAuthenticationTokenAsync(user, Constants.LoginProviderDefault, Constants.AccessToken, userResult.AccessToken);
            return userResult;
        }

        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AccountInfoResponse>> Register([FromBody] RegisterRequest request)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == request.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }
            if (await _userManager.Users.AnyAsync(x => x.UserName == request.Username))
            {
                ModelState.AddModelError("username", "Username taken");
                return ValidationProblem();
            }

            var user = new ApplicationUser
            {
                DisplayName = request.DisplayName,
                Email = request.Email,
                UserName = request.Username,
                IsAdmin = false
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded) return BadRequest("Problem registering user");

            var origin = Request.Headers["origin"];
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var verifyUrl = $"{origin}/v1.0/accounts/verify-email?token={token}&email={user.Email}";
            var message = $"<p>Please click the below link to verify your email address:</p><p><a href='{verifyUrl}'>Click to verify email</a></p>";

            //await _sendGridEmailSender.SendEmailAsync(user.Email, "Please verify email", message);
            var mailInfo = new EmailRequest
            {
                Subject = "Please verify email",
                Recipients = new List<string> { user.Email },
                HtmlBody = message
            };
            _emailService.SendSmtpMail(mailInfo);

            return Ok("Registration success. Please verify email.");
        }

        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token, [FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized();
            var decodedTokenBytes = WebEncoders.Base64UrlDecode(token);
            var decodedToken = Encoding.UTF8.GetString(decodedTokenBytes);
            var result = await _userManager.ConfirmEmailAsync(user, decodedToken);

            if (!result.Succeeded) return BadRequest("Could not verify email address");

            return Ok("Email confirmed - you can now login");
        }

        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        [HttpGet("resend-email-confirmation-link")]
        public async Task<IActionResult> ResendEmailConfirmationLink([FromQuery] string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return Unauthorized();

            var origin = Request.Headers["origin"];
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var verifyUrl = $"{origin}/v1.0/accounts/verify-email?token={token}&email={user.Email}";
            var message = $"<p>Please click the below link to verify your email address:</p><p><a href='{verifyUrl}'>Click to verify email</a></p>";

            //await _sendGridEmailSender.SendEmailAsync(user.Email, "Please verify email", message);
            var mailInfo = new EmailRequest
            {
                Subject = "Please verify email",
                Recipients = new List<string> { user.Email },
                HtmlBody = message
            };
            _emailService.SendSmtpMail(mailInfo);

            return Ok("Email verification link resent");
        }

        [MapToApiVersion("1.0")]
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<AccountInfoResponse>> GetCurrentUser()
        {
            var user = await _userManager.Users
                //.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
            //await SetRefreshToken(user);
            return await SetRefreshToken(user);
        }

        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        [HttpPost("fbLogin")]
        public async Task<ActionResult<AccountInfoResponse>> FacebookLogin(string accessToken)
        {
            var fbVerifyKeys = _config["Facebook:AppId"] + "|" + _config["Facebook:AppSecret"];

            var verifyToken = await _httpClient
                .GetAsync($"debug_token?input_token={accessToken}&access_token={fbVerifyKeys}");

            if (!verifyToken.IsSuccessStatusCode) return Unauthorized();

            var fbUrl = $"me?access_token={accessToken}&fields=name,email,picture.width(100).height(100)";

            var response = await _httpClient.GetAsync(fbUrl);

            if (!response.IsSuccessStatusCode) return Unauthorized();

            var fbInfo = JsonConvert.DeserializeObject<dynamic>(await response.Content.ReadAsStringAsync());

            var username = (string)fbInfo.id;

            var user = await _userManager.Users
                //.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == username);

            if (user != null) return await SetRefreshToken(user);

            user = new ApplicationUser
            {
                DisplayName = (string)fbInfo.name,
                Email = (string)fbInfo.email,
                UserName = (string)fbInfo.id,
                //Photos = new List<Photo>
                //{
                //    new Photo
                //    {
                //        Id = "fb_" + (string)fbInfo.id,
                //        Url = (string)fbInfo.picture.data.url,
                //        IsMain = true
                //    }}
            };

            user.EmailConfirmed = true;

            var result = await _userManager.CreateAsync(user);

            if (!result.Succeeded) return BadRequest("Problem creating user account");

            //await SetRefreshToken(user);
            return await SetRefreshToken(user);
        }

        [MapToApiVersion("1.0")]
        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<AccountInfoResponse>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _userManager.Users
                .Include(r => r.RefreshTokens)
                //.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            if (user == null) return Unauthorized();

            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            if (oldToken != null && !oldToken.IsActive) return Unauthorized("Invalid token");

            return await SetRefreshToken(user);
        }

        /// <summary>
        /// Thu hồi token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenRequest request)
        {
            // accept token from request body or cookie
            var token = request.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });

            var response = await _tokenService.RevokeToken(token, IpAddress());

            if (!response)
                return NotFound(new { message = "Token not found" });

            return Ok(new { message = "Token revoked" });
        }

        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
            await _userManager.RemoveAuthenticationTokenAsync(user, Constants.LoginProviderDefault, Constants.AccessToken);

            // Delete authentication cookie
            await HttpContext.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            //await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            return Ok("Logout successfully");
        }

        /// <summary>
        /// Gửi email đến user để cài đặt lại mật khẩu
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] EmailAccountRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            var jwt = _tokenService.CreateResetPasswordToken();
            await _userManager.SetAuthenticationTokenAsync(user, Constants.LoginProviderDefault, Constants.ResetPasswordToken, jwt);
            await _userManager.RemovePasswordAsync(user);
            var link = _config["ResetPasswordUrl"] + "?userId=" + user.Id + "&username=" + user.UserName + "&token=" + jwt;
            _emailService.SendSmtpMail(new EmailRequest
            (
                subject: "[Social Network] Reset password",
                recipients: new List<string> { user.Email },
                htmlBody: string.Format("Mật khẩu của bạn vừa được cài đặt lại. </br>Click vào link dưới đây để thay đổi mật khẩu: </br>{0}", link)
            ));

            return Ok("Reset password success");
        }

        /// <summary>
        /// Cài đặt mật khẩu mới
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [MapToApiVersion("1.0")]
        [HttpPost("reset-new-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetNewPassword([FromBody] ResetPasswordRequest request)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == request.UserId);
            if (user == null)
            {
                return NotFound("Account not exist");
            }

            var tokenDb = await _userManager.GetAuthenticationTokenAsync(user, Constants.LoginProviderDefault, Constants.ResetPasswordToken);
            if (tokenDb == null || tokenDb != request.Token)
            {
                return NotFound("Token not exist");
            }

            if (!_tokenService.IsValidResetPasswordToken(request.Token))
            {
                return NotFound("Token not exist");
            }
            var result = await _userManager.AddPasswordAsync(user, request.NewPassword);
            if (result.Errors.Any())
                throw new IdentityValidationException("Reset password not success");

            await _userManager.RemoveAuthenticationTokenAsync(user, Constants.LoginProviderDefault, Constants.ResetPasswordToken);
            return Ok("Reset password success");
        }

        private async Task<AccountInfoResponse> SetRefreshToken(ApplicationUser user)
        {
            var refreshToken = await _tokenService.GenerateRefreshToken(user, IpAddress());

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            return new AccountInfoResponse
            {
                Username = user.UserName,
                DisplayName = user.DisplayName,
                //Image = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                AccessToken = _tokenService.CreateJwtToken(user),
                RefreshToken = refreshToken.Token
            };
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}