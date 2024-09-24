using System;
using System.Collections.Generic;
using Adf.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {

        public static void ConfigureAppSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            var adfSettings = configuration.GetSection("Adf").Get<AdfOptions>();
            string APIVersion = adfSettings.Version.ToString();
            if (string.IsNullOrEmpty(APIVersion))
            {
                APIVersion = "1.0";
            }

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = adfSettings.ApplicationName, Version = APIVersion });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                var security = new Dictionary<string, IEnumerable<string>>
                    {
                    {"Bearer", new string[] { }},
                    };
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                            new string[] {  }
                    }
                    });
            }).AddSwaggerGenNewtonsoftSupport();

        }

    }
}
