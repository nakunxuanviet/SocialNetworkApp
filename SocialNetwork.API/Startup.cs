using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SocialNetwork.API.Filters;
using SocialNetwork.API.Filters.SwaggerFilters;
using SocialNetwork.Application;
using SocialNetwork.Application.Common.Interfaces;
using SocialNetwork.Infrastructure;
using SocialNetwork.Infrastructure.Persistence;
using SocialNetwork.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SocialNetwork.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication();
            services.AddCustomDbContext(Configuration);
            services.AddRepositories();
            services.AddServices();
            services.AddCustomIdentity();

            services.AddSingleton<ICurrentUserService, CurrentUserService>();

            services.AddHttpContextAccessor();

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddControllers(options =>
                options.Filters.Add(new ApiExceptionFilterAttribute()))
                    .AddFluentValidation();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            });

            services.AddSwaggerGen(c =>
            {
                //Following code to avoid swagger generation error
                //due to same method name in different versions.
                c.ResolveConflictingActions(descriptions =>
                {
                    return descriptions.First();
                });

                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "SocialNetwork.API", Version = "1.0" });
                c.SwaggerDoc("1.0", new OpenApiInfo { Title = "SocialNetwork.API", Version = "1.0" });
                c.SwaggerDoc("2.0", new OpenApiInfo { Title = "SocialNetwork.API", Version = "2.0" });

                // Remove param and replace version
                c.OperationFilter<RemoveVersionFromParameter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SocialNetwork.API v1"));
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/1.0/swagger.json", "SocialNetwork.API v1.0");
                    c.SwaggerEndpoint("/swagger/2.0/swagger.json", "SocialNetwork.API v2.0");
                    //c.RoutePrefix = string.Empty;
                });
            }

            app.UseHealthChecks("/health");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}