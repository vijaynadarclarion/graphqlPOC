using Adf.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Results;
using HotChocolate;

namespace Najm.GraphQL.WebAPI.Extensions;

public class ExceptionMiddlewareExtensions
{
    /// <summary>
    /// The _next.
    /// </summary>
    private readonly RequestDelegate _next;

    private readonly ILoggerFactory _loggerFactory;
    private readonly IConfiguration _configuration;
    /// <summary>
    /// the error
    /// </summary>



    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalizationMiddleware"/> class.
    /// </summary>
    /// <param name="next">
    /// The next.
    /// </param>
    public ExceptionMiddlewareExtensions(RequestDelegate next, ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        _next = next;
        _loggerFactory = loggerFactory;
        _configuration = configuration;
       // _localizer = localizer;
    }

    /// <summary>
    /// The invoke.
    /// </summary>
    /// <param name="context">
    /// The context.
    /// </param>
    /// <returns>
    /// The <see cref="Task"/>.
    /// </returns>
    public async Task InvokeAsync(HttpContext context)
    {
        string requestPayload = await FormatRequestAsync(context.Request);

        try
        {
            await _next(context);
            
           if (context.Items.ContainsKey("GraphQLHasAuthorizationError"))
            {
                int i = 0;
              //  context.Response.StatusCode = StatusCodes.Status403Forbidden;
            }
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, requestPayload);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception, string requestPayload)
    {
        var error = new ReturnResult<List<string>>();
        context.Response.ContentType = "application/json";
        var _logger = _loggerFactory.CreateLogger<ExceptionMiddlewareExtensions>();
        var applicationId = _configuration.GetValue<short>("AppSettings:ApplicationId");

        string userAgent = context.Request.Headers["User-Agent"].ToString();
        (string browserName, string browserVersion) = GetBrowserInfo(userAgent);
        string ipAddress = Convert.ToString(context.Connection?.RemoteIpAddress);
        var request = requestPayload;
        //string request = context.Request.ToString();
        var response = JsonSerializer.Serialize(error).ToString();

        if (exception.Source == "GraphQLException")
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            GraphQLException validationException = (GraphQLException)exception;
            List<ValidationFailure> validationFailures = (List<ValidationFailure>)validationException.Errors;
            foreach (var validationFailure in validationFailures)
            {
                error.Errors.Add(new Item(validationFailure.ErrorCode, validationFailure.ErrorMessage));
            }

            _logger.LogError(exception, "Error in RequestURL:{RequestURL} IPAddress:{IPAddress} Id:{Id} Message:{Message} ApplicationID:{ApplicationID} HttpVerb:{HttpVerb} RequestURL:{RequestURL} MachineName:{MachineName} UserName:{UserName} RequestId:{RequestId} SessionId:{SessionId} OperatingSystem:{OperatingSystem} BrowserName:{BrowserName} BrowserVersion:{BrowserVersion} Action:{Action} Request:{Request} Response:{Response} CreatedDate:{CreatedDate} CreatedDay:{CreatedDay}  CreatedMonth:{CreatedMonth} CreatedYear:{CreatedYear}", string.Format("{0}://{1}{2}{3}", context.Request.Scheme, context.Request.Host, context.Request.Path, context.Request.QueryString), ipAddress, Guid.NewGuid(), exception.Message, applicationId, context.Request.Method, string.Format("{0}://{1}", context.Request.Scheme, context.Request.Host), Environment.MachineName, context.User.Identity?.Name, Guid.NewGuid(), Guid.NewGuid(), RuntimeInformation.OSDescription, browserName, browserVersion, context.Request.Path, request, response, DateTime.Now, DateTime.Now.Date.Day, DateTime.Now.Date.Month, DateTime.Now.Date.Year);

        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            error.Errors.Add(new Item("CFP_" + Convert.ToString(context.Response.StatusCode), "Error occurred please check the message"));

            _logger.LogError(exception, "Error in RequestURL:{RequestURL} IPAddress:{IPAddress} Id:{Id} Message:{Message} ApplicationID:{ApplicationID} HttpVerb:{HttpVerb} RequestURL:{RequestURL} MachineName:{MachineName} UserName:{UserName} RequestId:{RequestId} SessionId:{SessionId} OperatingSystem:{OperatingSystem} BrowserName:{BrowserName} BrowserVersion:{BrowserVersion} Action:{Action} Request:{Request} Response:{Response} CreatedDate:{CreatedDate} CreatedDay:{CreatedDay}  CreatedMonth:{CreatedMonth} CreatedYear:{CreatedYear}", string.Format("{0}://{1}{2}{3}", context.Request.Scheme, context.Request.Host, context.Request.Path, context.Request.QueryString), ipAddress, Guid.NewGuid(), exception.Message, applicationId, context.Request.Method, string.Format("{0}://{1}", context.Request.Scheme, context.Request.Host), Environment.MachineName, context.User.Identity?.Name, Guid.NewGuid(), Guid.NewGuid(), RuntimeInformation.OSDescription, browserName, browserVersion, context.Request.Path, request, response, DateTime.Now, DateTime.Now.Date.Day, DateTime.Now.Date.Month, DateTime.Now.Date.Year);
        }


        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(error, options));
    }

    private async Task<string> FormatRequestAsync(HttpRequest request)
    {
        request.EnableBuffering();
        var buffer = new byte[Convert.ToInt32(request.ContentLength)];
        await request.Body.ReadAsync(buffer, 0, buffer.Length).ConfigureAwait(false);
        var bodyAsText = Encoding.UTF8.GetString(buffer);
        request.Body.Position = 0;
        return $"{bodyAsText}";
    }

    private (string browserName, string browserVersion) GetBrowserInfo(string userAgent)
    {
        // Implement your logic to parse the user-agent and extract the browser name and version
        // This can be done using regular expressions, string manipulation, or external libraries like UserAgentUtils, etc.
        // For simplicity, I'll demonstrate a simple example that works for certain cases.

        // For demonstration, assuming that user-agent contains the browser name before the first '/'
        int slashIndex = userAgent.IndexOf('/');
        if (slashIndex != -1)
        {
            string browserName = userAgent.Substring(0, slashIndex);
            string browserVersion = userAgent.Substring(slashIndex + 1);
            return (browserName, browserVersion);
        }

        // If the browser name and version are not found, return default values or null.
        return (string.Empty, string.Empty);
    }
}
