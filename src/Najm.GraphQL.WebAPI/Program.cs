using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Text;
using Adf.Core.AutoMapper;
using CorrelationId.DependencyInjection;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Services;
using Najm.GraphQL.ApplicationCore.Types;
using Najm.GraphQL.Infrastructure.Types;
using Najm.GraphQL.WebAPI.Extensions;
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




//Application code
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// Correlation ID
builder.Services.AddControllers();
builder.Services.AddDefaultCorrelationId();

builder.Services.AddSingleton(new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidIssuer = configuration["Jwt:Issuer"],
    ValidAudience = configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
});


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = configuration["Jwt:Issuer"],
        ValidAudience = configuration["Jwt:Audience"]
    };
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("https://localhost:5000")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials(); // Important for WebSockets
    });
});

// Automapper

builder.Services.AddTransient<IAccidentService, AccidentService>();
builder.Services.AddHttpClient();


builder.Services.AddGraphQLServer()  
    //.AddAuthorization()
    .RegisterService<AccidentService>()
   // .AddSocketSessionInterceptor<SocketSessionInterceptor>()
    .AddQueryType<QueryType>()
    .AddType<AccidentInfoType>()   
    .AddSubscriptionType<Subscription>()
    .AddInMemorySubscriptions()
    .AddType<AccidentsInputType>();

var app = builder.Build();

// Enable HTTPS redirection in production
//app.UseHttpsRedirection(); // Ensure this is enabled if you want to enforce HTTPS

// Enable authentication and authorization
app.UseAuthentication(); // Middleware to handle authentication

// Custom middleware for exception handling
app.UseMiddleware<ExceptionMiddlewareExtensions>();

// Enable WebSocket support
app.UseWebSockets(); // Ensure this is placed before routing

// Routing configuration
app.UseRouting(); // Add routing middleware
app.UseAuthorization();  // Middleware to handle authorization

app.UseEndpoints(endpoints =>
{
    // Map the GraphQL endpoint
    endpoints.MapGraphQL();//.RequireAuthorization(); // Enforce authorization if needed

    // Map the GraphQL Playground endpoint
    endpoints.MapGraphQLPlayground("/playground", new GraphQL.Server.Ui.Playground.PlaygroundOptions()
    {
        GraphQLEndPoint = "/graphql",
        SchemaPollingEnabled = false
    }).AllowAnonymous(); // Allow anonymous access to Playground if needed
});

// Run the application
app.Run();

