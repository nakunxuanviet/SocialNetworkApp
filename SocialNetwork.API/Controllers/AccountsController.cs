using System;
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
using SocialNetwork.Application.Common;
using SocialNetwork.Application.Common.Exceptions;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Emails;
using SocialNetwork.Domain.Entities.Accounts;

namespace SocialNetwork.API.Controllers
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

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.Users
                //.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Email == loginDto.Email);

            if (user == null) return Unauthorized("Invalid email");

            if (user.UserName == "admin") user.EmailConfirmed = true;

            if (!user.EmailConfirmed) return Unauthorized("Email not confirmed");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (result.Succeeded)
            {
                await SetRefreshToken(user);
                var userObj = CreateUserObject(user);

                // Save token
                await _userManager.SetAuthenticationTokenAsync(user, Constants.LoginProviderDefault, Constants.AccessToken, userObj.Token);
                return userObj;
            }

            return Unauthorized("Invalid password");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email taken");
                return ValidationProblem();
            }
            if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
            {
                ModelState.AddModelError("username", "Username taken");
                return ValidationProblem();
            }

            var user = new ApplicationUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Username,
                IsAdmin = false
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.Users
                //.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.Email == User.FindFirstValue(ClaimTypes.Email));
            await SetRefreshToken(user);
            return CreateUserObject(user);
        }

        [AllowAnonymous]
        [HttpPost("fbLogin")]
        public async Task<ActionResult<UserDto>> FacebookLogin(string accessToken)
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

            if (user != null) return CreateUserObject(user);

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

            await SetRefreshToken(user);
            return CreateUserObject(user);
        }

        [Authorize]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var user = await _userManager.Users
                .Include(r => r.RefreshTokens)
                //.Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.UserName == User.FindFirstValue(ClaimTypes.Name));

            if (user == null) return Unauthorized();

            var oldToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

            if (oldToken != null && !oldToken.IsActive) return Unauthorized();

            return CreateUserObject(user);
        }

        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutModel model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);
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
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] EmailAccountDto model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == model.Email);

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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("reset-new-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetNewPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null)
            {
                return NotFound("Account not exist");
            }

            var tokenDb = await _userManager.GetAuthenticationTokenAsync(user, Constants.LoginProviderDefault, Constants.ResetPasswordToken);
            if (tokenDb == null || tokenDb != model.Token)
            {
                return NotFound("Token not exist");
            }

            if (!_tokenService.IsValidResetPasswordToken(model.Token))
            {
                return NotFound("Token not exist");
            }
            var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (result.Errors.Any())
                throw new IdentityValidationException("Reset password not success");

            await _userManager.RemoveAuthenticationTokenAsync(user, Constants.LoginProviderDefault, Constants.ResetPasswordToken);
            return Ok("Reset password success");
        }

        private async Task SetRefreshToken(ApplicationUser user)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }

        private UserDto CreateUserObject(ApplicationUser user)
        {
            return new UserDto
            {
                DisplayName = user.DisplayName,
                //Image = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Token = _tokenService.CreateToken(user),
                Username = user.UserName
            };
        }
    }
}