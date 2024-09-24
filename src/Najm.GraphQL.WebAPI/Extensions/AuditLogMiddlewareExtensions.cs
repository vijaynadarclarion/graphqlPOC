using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Najm.GraphQL.WebAPI.Extensions;

public interface IRequestLogInfo
{
    string UserId { get; }

    string ClientId { get; }

    string CorrelationId { get; }
}

public class RequestLogInfo : IRequestLogInfo
{
    public string UserId
    {
        get
        {
            return "1";
        }

    }
    public string? ClientId { get; }
    public string? CorrelationId { get; }

    public string? HttpVerb { get; set; }
    public string? Url { get; set; }
    public string? RequestBody { get; set; }
    public string? ResponseBody { get; set; }
    public DateTime? AuditDate { get; set; }
}

public class AuditLogMiddlewareExtensions
{
    private readonly RequestDelegate _next;
    private readonly ILoggerFactory _loggerFactory;
    public AuditLogMiddlewareExtensions(RequestDelegate next, ILoggerFactory loggerFactory)
    {
        _next = next;
        _loggerFactory = loggerFactory;
    }

    public async Task Invoke(HttpContext context)
    {
        //First, get the incoming request
        var request = await FormatRequestAsync(context.Request);

        //Copy a pointer to the original response body stream
        var originalBodyStream = context.Response.Body;

        //Create a new memory stream...
        using (var responseBody = new MemoryStream())
        {
            //...and use that for the temporary response body
            context.Response.Body = responseBody;

            ////Continue down the Middleware pipeline, eventually returning to this class
            await _next(context);

            //Format the response from the server
            var response = await FormatResponseAsync(context.Response);

            //TODO: Save log to chosen datastore
            if (context != null
                && context.Request.Method == HttpMethods.Post
                && context.Response.StatusCode != 500
                && context.Request.Path.Value.Contains("graphql")
                && !context.Request.Path.Value.Contains("token")
                && (!String.IsNullOrWhiteSpace(request) || !String.IsNullOrWhiteSpace(response))
                 )
            {
                SaveAuditlog(new RequestLogInfo()
                {

                    HttpVerb = context.Request?.Method,
                    Url = GetAbsoluteUrl(context.Request),
                    RequestBody = request,
                    ResponseBody = response,
                    AuditDate = DateTime.Now,
                });
            }

            //Copy the contents of the new memory stream (which contains the response) to the original stream, which is then returned to the client.
            await responseBody.CopyToAsync(originalBodyStream);
        }
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

    private async Task<string> FormatResponseAsync(HttpResponse response)
    {
        //We need to read the response stream from the beginning...
        response.Body.Seek(0, SeekOrigin.Begin);

        //...and copy it into a string
        string text = await new StreamReader(response.Body).ReadToEndAsync();

        //We need to reset the reader for the response so that the client can read it.
        response.Body.Seek(0, SeekOrigin.Begin);

        //Return the string for the response, including the status code (e.g. 200, 404, 401, etc.)

        return $"{response.StatusCode}: {text}";
    }

    private void SaveAuditlog(RequestLogInfo requestLogInfo)
    {
        var _logger = _loggerFactory.CreateLogger<AuditLogMiddlewareExtensions>();
        _logger.LogInformation("HttpVerb: {HttpVerb}, Url : {Url} RequestBody: {RequestBody} ResponseBody : {ResponseBody} AuditDate : {AuditDate}",
         requestLogInfo.HttpVerb, requestLogInfo.Url, requestLogInfo.RequestBody, requestLogInfo.ResponseBody, requestLogInfo.AuditDate);
    }
    public string GetAbsoluteUrl(HttpRequest httpRequest)
    {
        UriBuilder uriBuilder = new UriBuilder();
        uriBuilder.Scheme = httpRequest.Scheme;
        uriBuilder.Host = httpRequest.Host.Host;
        uriBuilder.Path = httpRequest.PathBase + httpRequest.Path.ToString();
        uriBuilder.Query = httpRequest.QueryString.ToString();
        return uriBuilder.Uri.AbsoluteUri;
    }

}
