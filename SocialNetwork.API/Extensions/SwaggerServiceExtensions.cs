using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SocialNetwork.API.Filters.SwaggerFilters;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SocialNetwork.API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //Following code to avoid swagger generation error due to same method name in different versions.
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
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Scheme = "Bearer"
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
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                         },
                            Array.Empty<string>()
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/1.0/swagger.json", "SocialNetwork.API v1.0");
                c.SwaggerEndpoint("/swagger/2.0/swagger.json", "SocialNetwork.API v2.0");
            });

            return app;
        }
    }
}
