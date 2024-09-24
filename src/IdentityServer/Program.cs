using Adf.OAuthServer;
using Adf.OAuthServer.EntityFramework.DbContexts;
using Adf.OAuthServer.EntityFramework.Mappers;
using IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;

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

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);


//var identityConnectionString = app.Configuration.GetConnectionString("CommonsDbConnection");
//var identityConnectionString = "Data Source=localhost; Initial Catalog=BlobStorage;Persist Security Info=True;User ID=sa;Password=SwN12345678;MultipleActiveResultSets=True";
var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;
const string connectionString = @"Data Source=CT00031\SQLEXPRESS;Initial Catalog=IdentityStore;Integrated Security=true;TrustServerCertificate=true;";

builder.Services.AddDbContext<PersistedGrantDbContext>(options =>
        options.UseSqlServer(connectionString)
               .EnableSensitiveDataLogging()  // Enable sensitive data logging
               .LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddIdentityServer()
    .AddTestUsers(TestUsers.Users)
    .AddConfigurationStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString, sql =>
        {
            sql.MigrationsAssembly(migrationsAssembly);
            // Configure the split query behavior
           // sql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        });
    })
    .AddOperationalStore(options =>
    {
        options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                 sql => sql.MigrationsAssembly(typeof(Program).Assembly.GetName().Name));


        // options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
        //  sql => sql.MigrationsAssembly(migrationsAssembly));
    })
    .AddDeveloperSigningCredential();

//builder.Services.AddDeveloperSigningCredential();



builder.Services.AddAuthentication()               
               .AddOpenIdConnect("oidc", "Demo IdentityServer", options =>
               {
                   options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                   options.SignOutScheme = IdentityServerConstants.SignoutScheme;
                   options.SaveTokens = true;

                   options.Authority = "https://demo.identityserver.io/";
                   options.ClientId = "interactive.confidential";
                   options.ClientSecret = "secret";
                   options.ResponseType = "code";
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       NameClaimType = "name",
                       RoleClaimType = "role"
                   };
               });

var app = builder.Build();
/*
using (var serviceScope = app.Services.GetService<IServiceScopeFactory>().CreateScope())
{
    serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

    var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
    context.Database.Migrate();
    if (!context.Clients.Any())
    {
        foreach (var client in Config.Clients)
        {
            context.Clients.Add(client.ToEntity());
        }
        context.SaveChanges();
    }

    if (!context.IdentityResources.Any())
    {
        foreach (var resource in Config.IdentityResources)
        {
            context.IdentityResources.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }

    if (!context.ApiScopes.Any())
    {
        foreach (var resource in Config.ApiScopes)
        {
            context.ApiScopes.Add(resource.ToEntity());
        }
        context.SaveChanges();
    }
}
*/
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseIdentityServer();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
*/
app.Run();

