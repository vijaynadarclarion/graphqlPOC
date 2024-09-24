using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.Data;
using Adf.Core.DateTimes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Adf.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddAdf(
            this IServiceCollection services, 
            IConfiguration configuration, 
            List<Assembly> assemblies)
        {
            // Adf settings Options
            services.Configure<AdfOptions>(configuration.GetSection("Adf"));            
            services.AddScoped<DateTimeHelperService>();
            //services.AddScoped<SettingsService>();

        }
    }
}
