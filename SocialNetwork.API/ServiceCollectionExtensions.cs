using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SocialNetwork.API.Filters;
using SocialNetwork.API.Filters.SwaggerFilters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SocialNetwork.API
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomController(this IServiceCollection services)
        {
            services.AddControllers(options =>
                options.Filters.Add(new ApiExceptionFilterAttribute()))
                    .AddFluentValidation();

            // Customise default API behaviour
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }

        public static IServiceCollection ConfigureCors(this IServiceCollection services)
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

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //Following code to avoid swagger generation error
                //due to same method name in different versions.
                c.ResolveConflictingActions(descriptions =>
                {
                    return descriptions.First();
                });

                c.SwaggerDoc("1.0", new OpenApiInfo { Title = "SocialNetwork.API", Version = "1.0" });
                c.SwaggerDoc("2.0", new OpenApiInfo { Title = "SocialNetwork.API", Version = "2.0" });

                // Remove param and replace version
                c.OperationFilter<RemoveVersionFromParameter>();
                c.DocumentFilter<ReplaceVersionWithExactValueInPath>();

                // Configure security swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "JWT Authorization header using token",
                    //Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          }
                        },
                        new string[] {}
                      }
                    });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }
    }
}