using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using Adf.Core;
using Adf.Core.AutoMapper;
using CorrelationId.DependencyInjection;
using HotChocolate.AspNetCore;
using HotChocolate.Data;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Dtos;
using Najm.GraphQL.ApplicationCore.Accidents.Services;
using Najm.GraphQL.ApplicationCore.Entity;
using Najm.GraphQL.ApplicationCore.Types;
using Najm.GraphQL.Infrastructure;
using Najm.GraphQL.Infrastructure.Accidents.Resolvers;
using Najm.GraphQL.Infrastructure.Data;
using Najm.GraphQL.Infrastructure.Types;
using Najm.GraphQL.WebAPI.Extensions;
using Newtonsoft.Json;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

Microsoft.Extensions.Configuration.ConfigurationManager configuration = builder.Configuration;

configuration.SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("serilog.json")
             //.AddJsonFile($"serilog.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true)
             .Build();

var logger = new LoggerConfiguration()
                   .ReadFrom.Configuration(configuration)
                   .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);


List<Assembly> _listOfAssemblies;

_listOfAssemblies = new List<Assembly> {
                // Application Assemblies
                typeof(Najm.GraphQL.Infrastructure.ServiceCollectionExtensions).Assembly,
            };

// ---------------------- ADF ----------------------------------------------------
builder.Services.AddAdf(configuration, _listOfAssemblies);
//Application code
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

//builder.Services.AddTransient<IJwtAuthenticationHandler, JwtAuthenticationHandler>();
// Correlation ID

builder.Services.AddDefaultCorrelationId();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "demo",
                ValidAudience = "demoapi",
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("nawlvTMgHZxe5Z9kWtrIyYxFVExw+7qH6iL7Kz4F7Iw+pxCGLFD4C2iDY7aW6zopnKMsxDRzbIkV7z+0mbVYzuT2mxYFDYLpV3H4huNfIVzAMTtVHCqSWzCTepv4+Tfl/AHorfw1AJ8jFBw58I3mOyR1/hlmFVnYDcrgMyCCMl7iemUdbESP1ahtF4inyvFQ66nWLSTg39J6lOm7vHyOCyh1wWftWDWxsdfqeIwwLRPCyiA0z+Pf9gEo7AVJggyZoIPnfAYzn6v9VEZ9raA0rgtqNOibFtinzpvVw7xLo99JtsrS9d4CIvXhJMK9adUhZzfE9ZStjhWJWn6CE1iWyg=="))
            };
        });

builder.Services.AddAuthorization(options =>
{

});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:7171")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Important for WebSockets
    });
});

// Automapper
builder.Services.RegisterMapperProfiles(_listOfAssemblies);
builder.Services.ConfigureInfrastructureServices(configuration);
builder.Services.AddTransient<IAccidentService, AccidentService>();

builder.Services.AddHttpClient();
builder.Services.AddTransient<IPolicyService, PolicyService>();
builder.Services.AddTransient<IPolicyResolver, PolicyResolver>();

// Assuming the JSON file is in the same directory as your executable
string filePath = "SchemaAuth\\schemasetting.json";
if (File.Exists(filePath))
{
    // Read the JSON file content
    string json = File.ReadAllText(filePath);
    AuthorizationConfig config = JsonConvert.DeserializeObject<AuthorizationConfig>(json);
    builder.Services.AddSingleton(config);
}



builder.Services.AddGraphQLServer()
    .AddAuthorization()
    .RegisterService<AccidentService>()
    .RegisterService<PolicyService>()    
    .RegisterDbContext<AppReadOnlyDbContext>(DbContextKind.Pooled)
    .AddErrorFilter<CustomErrorFilter>()
    .AddQueryType<QueryType>()
    .AddType<AccidentInfoType>()
    .AddType<AccidentPartyType>()
    .AddType<AccidentsInput>()
    .AddSubscriptionType<Subscription>()    
    .AddInMemorySubscriptions();

var app = builder.Build();

app.UseStaticFiles(); // Enable serving static files
app.UseMiddleware<ExceptionMiddlewareExtensions>();
app.UseMiddleware<AuditLogMiddlewareExtensions>();
app.UseMiddleware<SchemaSelectionMiddleware>();

app.UseWebSockets();
app.UseRouting();
app.UseCors();
app.UseMiddleware<SchemaSelectionMiddleware>();
app.MapGraphQL();
/* 
app.UseEndpoints(endpoints =>
{
    
    endpoints.MapGraphQL().WithOptions(new GraphQLServerOptions
    {
        Tool =
        {
        Enable = true
        }
    });

endpoints.MapGraphQLPlayground("/playground", new GraphQL.Server.Ui.Playground.PlaygroundOptions()
 {
     GraphQLEndPoint = "/graphql",
     SchemaPollingEnabled = false


 });
});*/

app.Run();

