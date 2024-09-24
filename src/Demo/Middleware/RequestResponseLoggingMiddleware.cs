using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileSystemGlobbing.Internal;

public class RequestResponseLoggingMiddleware
{
    private readonly RequestDelegate _next;
  //  private readonly RequestDelegate _next;
    public RequestResponseLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context,
        ILogger<RequestResponseLoggingMiddleware> logger, IEnumerable<ISchema> schemas)
    {
        context.Request.EnableBuffering();

        var buffer = new char[100];
        var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);
        int charCount = await reader.ReadAsync(buffer, 0, buffer.Length);

        // Convert the char array to a string, trimming any null characters
        string requestBodyPreview = new string(buffer, 0, charCount);


        if(requestBodyPreview.Length == 0 || requestBodyPreview.IndexOf("{\"operationName\":\"IntrospectionQuery\"") == -1)
        {
            return;
        }

        Dictionary<string, string> dicPattern = new Dictionary<string, string>();
        /*dicPattern.Add("Response\\__TypeSchemaResponse.json", "\"operationName\"\\s*:\\s*\"IntrospectionQuery\".*__type\\s*\\(name\\s*:\\s*\\\\\"__Schema\\\\\"");
        dicPattern.Add("Response\\__DirectiveResponse.json", "\"operationName\"\\s*:\\s*\"IntrospectionQuery\".*__type\\s*\\(name\\s*:\\s*\"__Directive\"\\)");
        dicPattern.Add("Response\\__InputValueResponse.json", "\"operationName\"\\s*:\\s*\"IntrospectionQuery\".*__type\\s*\\(name\\s*:\\s*\"__InputValue\"\\)");
        dicPattern.Add("Response\\__typeResponse.json", "\"operationName\"\\s*:\\s*\"IntrospectionQuery\".*__type\\s*\\(name\\s*:\\s*\"__Type\"\\)");
        dicPattern.Add("Response\\__SchemaResponse.json", "{\"operationName\":\"IntrospectionQuery\".*__schema");
        */
        foreach (var keyvalue in dicPattern)
        {
            Regex regex = new Regex(keyvalue.Value);

            if(regex.IsMatch(requestBodyPreview))
            {
                // The path to the JSON file
                var jsonFilePath = keyvalue.Key;

                // Read the JSON file
                var jsonResponse = await File.ReadAllTextAsync(jsonFilePath);

                // Set the response content type to application/json
                context.Response.ContentType = "application/json";

                // Write the JSON response to the HttpContext.Response
                await context.Response.WriteAsync(jsonResponse);

                // Stop further processing by not calling the next middleware
                return;
            }
        }
       
      

        await _next(context);

        /*
        // Capture the request body
        context.Request.EnableBuffering();
        var requestBodyStream = new MemoryStream();
        await context.Request.Body.CopyToAsync(requestBodyStream);
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        string requestBody = new StreamReader(requestBodyStream).ReadToEnd();
        requestBodyStream.Seek(0, SeekOrigin.Begin);
        context.Request.Body = requestBodyStream;

        // Capture the response body
        var originalResponseBodyStream = context.Response.Body;
        var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        // Continue down the Middleware pipeline, eventually returning here
        await _next(context);

        responseBodyStream.Seek(0, SeekOrigin.Begin);
        string responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
        responseBodyStream.Seek(0, SeekOrigin.Begin);
        await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        context.Response.Body = originalResponseBodyStream;

        // Log the request and response
        LogRequestAndResponse(context, requestBody, responseBody, logger);
        */
    }

   
}
