using AspNetCoreRateLimit;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.API.Infrastructure.Filters;
using System.Collections.Generic;
using System.Globalization;

namespace SocialNetwork.API.Infrastructure.ExtensionConfigureServices
{
    public static class ServiceCollectionConfigurations
    {
        public static IServiceCollection AddCustomController(this IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                //var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                //opt.Filters.Add(new AuthorizeFilter(policy));

                // Handle exceptions thrown by an action
                opt.Filters.Add(new ApiExceptionFilterAttribute());
            }).AddFluentValidation().AddDataAnnotationsLocalization();

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

        public static IServiceCollection AddAndConfigureApiVersioning(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options => { options.LowercaseUrls = true; });

            services.AddApiVersioning(setup =>
            {
                //setup.DefaultApiVersion = new ApiVersion(1, 0);
                //setup.AssumeDefaultVersionWhenUnspecified = true;

                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                setup.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }

        public static IServiceCollection AddAndConfigureLocalization(this IServiceCollection services)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            var supportedCultures = new List<CultureInfo> { new("en"), new("vi") };
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("vi");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            return services;
        }
    }
}