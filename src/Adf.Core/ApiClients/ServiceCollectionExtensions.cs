using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adf.Core.ApiClients.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;

namespace Adf.Core.ApiClients
{
    
    public static class ServiceCollectionExtensions
    {
        private const string PoliciesConfigurationSectionName = "PollyPolicies";

        /// <summary>
        /// implementation for AddPollyPolicies. 
        /// It starts by setting up and reading a configuration section in our appsettings.json file of type PolicyOptions. 
        /// Then adds the PolicyRegistry which is where Polly stores it's policies. 
        /// Finally, we add a retry and circuit breaker policy and configure them using the settings we've read from the PolicyOptions.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <param name="configurationSectionName"></param>
        /// <returns></returns>
        public static IServiceCollection AddPollyPolicies(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSectionName = PoliciesConfigurationSectionName)
        {
            services.Configure<PollyPolicyOptions>(configuration);
            var policyOptions = configuration.GetSection(configurationSectionName).Get<PollyPolicyOptions>();

            if(policyOptions == null)
            {
                return services;
            }

            var policyRegistry = services.AddPolicyRegistry();
            
            policyRegistry.Add(
                PolicyName.HttpRetry,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .WaitAndRetryAsync(
                        policyOptions.HttpRetry.Count,
                        retryAttempt => TimeSpan.FromSeconds(Math.Pow(policyOptions.HttpRetry.BackoffPower, retryAttempt))));
            
            policyRegistry.Add(
                PolicyName.HttpCircuitBreaker,
                HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .CircuitBreakerAsync(
                        handledEventsAllowedBeforeBreaking: policyOptions.HttpCircuitBreaker.ExceptionsAllowedBeforeBreaking,
                        durationOfBreak: policyOptions.HttpCircuitBreaker.DurationOfBreak));

            return services;
        }

        /// <summary>
        /// The AddHttpClient method starts by binding the TClientOptions type to a configuration section in appsettings.json. 
        /// TClientOptions is a derived type of HttpClientOptions which just contains a base address and time-out value.
        /// </summary>
        public static IServiceCollection AddHttpClient<TClient, TImplementation, TClientOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationSectionName)
            where TClient : class
            where TImplementation : class, TClient
            where TClientOptions : HttpClientOptions, new() =>
            services
                .Configure<TClientOptions>(configuration.GetSection(configurationSectionName))
                .AddSingleton<CorrelationIdDelegatingHandler>()
                .AddSingleton<UserAgentDelegatingHandler>()
                .AddHttpClient<TClient, TImplementation>()
                .ConfigureHttpClient(
                    (serviceProvider, httpClient) =>
                    {
                        var httpClientOptions = serviceProvider
                            .GetRequiredService<IOptions<TClientOptions>>()
                            .Value;
                        httpClient.BaseAddress = httpClientOptions.BaseAddress;
                        httpClient.Timeout = httpClientOptions.Timeout;
                    })
                .ConfigurePrimaryHttpMessageHandler(x => new DefaultHttpClientHandler())
                .AddPolicyHandlerFromRegistry(PolicyName.HttpRetry)
                .AddPolicyHandlerFromRegistry(PolicyName.HttpCircuitBreaker)
                .AddHttpMessageHandler<CorrelationIdDelegatingHandler>()
                .AddHttpMessageHandler<UserAgentDelegatingHandler>()
                .Services;
    }
}
