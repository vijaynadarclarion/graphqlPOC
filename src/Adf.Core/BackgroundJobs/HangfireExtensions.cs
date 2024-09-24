using Hangfire;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Adf.Core.BackgroundJobs
{
    public static class HangfireExtensions
    {
        public static void RegisterBackGroundJobs(this IApplicationBuilder app, List<Assembly> assemblies, IServiceProvider serviceProvider, IServiceCollection services)
        {
            app.UseHangfireDashboard("/hangfire",
                new DashboardOptions
                {
                    Authorization = new[] { new HangfireDashboardAuthFilter() }
                });

            app.UseHangfireServer();

            foreach (var assembly in assemblies)
            {
                //var service = services.Where(t => t.ServiceType == typeof(IBackgroundJob)).ToList();

                //foreach (var item in services)
                //{
                //    var x =  item.ServiceType;

                //    RecurringJob.AddOrUpdate(() => x.Execute(), x.CronExpression);
                //}
                foreach (Type mytype in assembly.GetTypes()
                                                .Where(mytype => mytype.GetInterfaces()
                                                .Contains(typeof(IBackgroundJob))))
                {
                    //IBackgroundJob backGroundJob = (IBackgroundJob)serviceProvider.GetService(mytype);

                    IBackgroundJob backGroundJob = (IBackgroundJob)Activator.CreateInstance(mytype, serviceProvider);

                    RecurringJob.AddOrUpdate(() => backGroundJob.Execute(), backGroundJob.CronExpression);

                }
            }
        }
        //public static void AddBackGroundJobs(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        //{
        //    //services.AddScoped(typeof(IBackgroundJob), typeof(BackGroundJobBase));

        //    if (configuration.MockDataBase())
        //    {
        //        services.AddHangfire(config =>
        //        {
        //            config.UseMemoryStorage();
        //        });

        //    }
        //    else
        //    {
        //        var conn = configuration.GetConnectionString("HangfireConnection");

        //        services.AddHangfire(x => x.UseSqlServerStorage(conn));
        //    }

        //}
        public static T GetServiceForHangfire<T>(this IServiceProvider serviceProvider)
        {
            var instance = (T)serviceProvider.GetService(typeof(T));

            return instance;
        }
    }
}
