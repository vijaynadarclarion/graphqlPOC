using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Services;
using Najm.GraphQL.ApplicationCore.Interfaces;
using Najm.GraphQL.Infrastructure.Accidents.Resolvers;
using Najm.GraphQL.Infrastructure.Data;

namespace Najm.GraphQL.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {

            // EF ReadOnly DbContext configuration
            var appReadOnlyConnectionString = configuration.GetConnectionString("AppDbReadOnlyConnection");
           // services.AddDbContext<AppReadOnlyDbContext>(options =>
            //    options.UseSqlServer(appReadOnlyConnectionString, opt => opt.CommandTimeout(60)));
          
            services.AddPooledDbContextFactory<AppReadOnlyDbContext>(options =>
                options.UseSqlServer(appReadOnlyConnectionString,
                opt => {
                    opt.CommandTimeout(5000);
                    opt.EnableRetryOnFailure();
                }));

            services.AddTransient<IAppReadOnlyDbContext, AppReadOnlyDbContext>();
            services.AddTransient(typeof(IReadOnlyRepository<>), typeof(PooledReadOnlyRepository<>));
            services.AddTransient<IPolicyResolver, PolicyResolver>();

            // services.AddTransient<IAccidentService, AccidentService>();
        }
    }
}
