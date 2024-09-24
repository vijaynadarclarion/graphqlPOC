using Demo;
using Demo.Query;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Serilog;

Environment.SetEnvironmentVariable("HOTCHOCOLATE_TELEMETRY_OPTOUT", "1");

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((hostingContext, loggerConfiguration) =>
 {
     loggerConfiguration
     .ReadFrom.Configuration(hostingContext.Configuration)
     .Enrich.FromLogContext();
 });
var configuration = builder.Configuration;

var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = "demo",
    ValidAudience = "demoapi",
    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("nawlvTMgHZxe5Z9kWtrIyYxFVExw+7qH6iL7Kz4F7Iw+pxCGLFD4C2iDY7aW6zopnKMsxDRzbIkV7z+0mbVYzuT2mxYFDYLpV3H4huNfIVzAMTtVHCqSWzCTepv4+Tfl/AHorfw1AJ8jFBw58I3mOyR1/hlmFVnYDcrgMyCCMl7iemUdbESP1ahtF4inyvFQ66nWLSTg39J6lOm7vHyOCyh1wWftWDWxsdfqeIwwLRPCyiA0z+Pf9gEo7AVJggyZoIPnfAYzn6v9VEZ9raA0rgtqNOibFtinzpvVw7xLo99JtsrS9d4CIvXhJMK9adUhZzfE9ZStjhWJWn6CE1iWyg=="))
};
builder.Services.AddSingleton(tokenValidationParameters);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = tokenValidationParameters;
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync("Unauthorized");
        }
    };
});


builder.Services.AddAuthorization();



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

builder.Services
        .AddGraphQLServer()
        .AddAuthorization()
        .AddQueryType<QueryType>()
        .AddType<CompQuotesResponseType>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles(); // Enable serving static files

app.UseRouting();
//app.MapBananaCakePop
//app.MapBananaCakePop();
//app.MapGraphQL();
app.UseCors();
//app.UseMiddleware<CustomAuthorizationMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGraphQL().WithOptions(new GraphQLServerOptions
    {
        Tool =
      {
        Enable = false
      }
    });
  
    endpoints.MapGraphQLPlayground("/playground", new GraphQL.Server.Ui.Playground.PlaygroundOptions()
    {
        GraphQLEndPoint = "/graphql"
        
       
    });
});


app.Run();
