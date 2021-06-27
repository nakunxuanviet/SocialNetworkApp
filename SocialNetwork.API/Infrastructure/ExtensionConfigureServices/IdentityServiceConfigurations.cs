using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.Application.Accounts.Models;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Domain.Entities.ApplicationRoles;
using SocialNetwork.Domain.Entities.ApplicationUsers;
using SocialNetwork.Infrastructure.Identity;
using SocialNetwork.Infrastructure.Persistence;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.API.Infrastructure.ExtensionConfigureServices
{
    public static class IdentityServiceConfigurations
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentityCore<ApplicationUser>(opt =>
            {
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(Convert.ToInt32(configuration["LockAccount:DefaultLockoutMinutes"]));
                opt.Lockout.MaxFailedAccessAttempts = Convert.ToInt32(configuration["LockAccount:MaxFailedAccessAttempts"]);
                opt.Password.RequireNonAlphanumeric = false;
                opt.SignIn.RequireConfirmedEmail = true;
                opt.User.RequireUniqueEmail = false;
            })
            .AddRoles<ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                // Identity made Cookie authentication the default. However, now JWT Bearer Auth to be the default.
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            //.AddCookie(options =>
            // {
            //     options.Cookie.HttpOnly = true;
            //     options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //     options.Cookie.SameSite = SameSiteMode.Lax;
            // })
            //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
            //.AddCookie(IdentityConstants.ApplicationScheme)
            .AddJwtBearer(opt =>
            {
                // Configure the Authority to the expected value for your authentication provider
                // This ensures the token is appropriately validated
                // var identityUrl = Configuration.GetValue<string>("IdentityUrl");
                // options.Authority = identityUrl;

                opt.RequireHttpsMetadata = false;
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // We have to hook the OnMessageReceived event in order to
                // allow the JWT authentication handler to read the access
                // token from the query string when a WebSocket or
                // Server-Sent Events request comes in.
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        // If the request is for our hub...
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                        {
                            // Read the token out of the query string
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsActivityHost", policy =>
                {
                    policy.Requirements.Add(new IsHostRequirement());
                });
            });

            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.Configure<JwtResetPasswordOptions>(configuration.GetSection("ResetPasswordJwt"));

            services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();
            services.AddTransient<IJwtService, JwtService>();

            return services;
        }
    }
}