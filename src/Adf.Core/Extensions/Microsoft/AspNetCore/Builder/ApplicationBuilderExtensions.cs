using System;
using Adf.Core;
using Microsoft.Extensions.Configuration;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        public static void ConfigureCustomSwaggerMiddleware(this IApplicationBuilder app, IConfiguration configuration)
        {
            var adfSettings = configuration.GetSection("Adf").Get<AdfOptions>();

            if (Convert.ToBoolean(adfSettings.Swagger.Enable))
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    string RouteName = adfSettings.Swagger.RouteName.ToString();
                    if (string.IsNullOrEmpty(RouteName))
                    {
                        c.SwaggerEndpoint("/swagger/v1/swagger.json", adfSettings.ApplicationName);
                        c.InjectStylesheet("/Files/swagger.css");
                        c.InjectJavascript("/Files/swagger.js", "text/javascript");
                    }
                    else
                    {
                        c.SwaggerEndpoint($"/{RouteName}/swagger/v1/swagger.json", adfSettings.ApplicationName);
                        c.InjectStylesheet($"/{RouteName}/Files/swagger.css");
                        c.InjectJavascript($"/{RouteName}/Files/swagger.js", "text/javascript");
                    }
                });
            }
        }

    }
}
