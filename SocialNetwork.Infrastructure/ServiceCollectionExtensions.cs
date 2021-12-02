using Hangfire;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.Application.Common.Behaviours;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Application.Common.Models.Cache;
using SocialNetwork.Application.Common.Models.Emails;
using SocialNetwork.Domain.Entities.Idempotency;
using SocialNetwork.Domain.Interfaces;
using SocialNetwork.Domain.SeedWork;
using SocialNetwork.Infrastructure.Cache;
using SocialNetwork.Infrastructure.Email;
using SocialNetwork.Infrastructure.Files;
using SocialNetwork.Infrastructure.Idempotency;
using SocialNetwork.Infrastructure.Identity;
using SocialNetwork.Infrastructure.Logging;
using SocialNetwork.Infrastructure.Persistence;
using SocialNetwork.Infrastructure.Repository;
using SocialNetwork.Infrastructure.Services;
using System;

namespace SocialNetwork.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("SocialDb"));
            }
            else
            {
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    //options.UseLazyLoadingProxies();
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
                });
            }

            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepositoryBase<>), typeof(EfRepositoryBase<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IRequestManager, RequestManager>(); //Idempotency

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IDomainEventService, DomainEventService>();
            services.AddTransient<ISystemDateTime, SystemDateTimeService>();
            services.AddTransient<ICsvFileBuilder, CsvFileBuilder>();
            services.AddSingleton<IUserAccessor, UserAccessor>();
            services.AddTransient<IIdentityService, IdentityService>();

            return services;
        }

        public static IServiceCollection AddCustomLogger(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILoggerManager<>), typeof(LoggerAdapter<>));

            return services;
        }

        public static IServiceCollection AddEmail(this IServiceCollection services, IConfiguration configuration)
        {
            // Smtp
            services.AddSingleton<IBodyHtmlGenerator, BodyHtmlGenerator>();
            services.AddSingleton<IMailSender, MailSender>();
            services.AddSingleton<ISmtpMailService, SmtpMailService>();

            // SendGrid
            services.Configure<SendGridEmailSenderOptions>(configuration.GetSection("SendGrid"));
            services.AddScoped<ISendGridEmailService, SendGridEmailService>();

            return services;
        }

        public static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
        {
            //Installing Redis Server: docker run -d -p 6379:6379 --name myredis redis

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = $"{configuration.GetValue<string>("Redis:Server")}:{configuration.GetValue<int>("Redis:Port")}";
            });
            services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));
            services.AddSingleton<ICacheService, RedisCacheService>();

            return services;
        }

        // Repository pattern with caching and hangfire
        public static IServiceCollection AddRepoPatternCachingHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<CacheConfiguration>(configuration.GetSection("CacheConfiguration"));
            //services.AddMemoryCache();   // For In-Memory Caching
            services.AddTransient<MemoryCacheService>();
            services.AddTransient<RedisCacheService>();
            services.AddTransient<Func<CacheTech, ICacheService>>(serviceProvider => key =>
            {
                switch (key)
                {
                    case CacheTech.Memory:
                        services.AddMemoryCache();
                        return serviceProvider.GetService<MemoryCacheService>();

                    case CacheTech.Redis:
                        services.AddStackExchangeRedisCache(options =>
                        {
                            options.Configuration = $"{configuration.GetValue<string>("Redis:Server")}:{configuration.GetValue<int>("Redis:Port")}";
                        });
                        return serviceProvider.GetService<RedisCacheService>();

                    default:
                        services.AddMemoryCache();
                        return serviceProvider.GetService<MemoryCacheService>();
                }
            });

            return services;
        }

        // Response Caching with MediatR Pipeline Behavior
        public static IServiceCollection AddCachingWithMediatR(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDistributedMemoryCache();
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));

            return services;
        }

        public static IServiceCollection AddConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();

            return services;
        }
    }
}