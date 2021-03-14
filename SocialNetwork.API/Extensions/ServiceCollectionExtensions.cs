using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.API.Configures;
using SocialNetwork.API.Filters;
using System.Collections.Generic;

namespace SocialNetwork.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomController(this IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));

                // Handle exceptions thrown by an action
                opt.Filters.Add(new ApiExceptionFilterAttribute());
            }).AddFluentValidation();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("WWW-Authenticate", "Pagination")
                        .WithExposedHeaders("Content-Disposition")
                        .WithOrigins("http://localhost:3000");
                });
            });

            return services;
        }

        public static IServiceCollection AddRateLimit(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // This will set up the 500 requests per minute and 3,600 requests per hour that I already mentioned.
            services.Configure<ClientRateLimitOptions>(options =>
            {
                options.GeneralRules = new List<RateLimitRule>
                {
                    new RateLimitRule
                    {
                        Endpoint = "*",
                        Period = "1m",
                        Limit = 500,
                     },
                    new RateLimitRule
                     {
                        Endpoint = "*",
                        Period = "1h",
                        Limit = 3600,
                    }
                };
            });

            services.AddSingleton<IRateLimitConfiguration, ApiRateLimitConfiguration>();

            return services;
        }
    }
}