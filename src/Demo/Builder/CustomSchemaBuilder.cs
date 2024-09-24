using HotChocolate.AspNetCore.Instrumentation;
using HotChocolate.AspNetCore.Serialization;
using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using HotChocolate.Utilities;
using Microsoft.AspNetCore.Http.HttpResults;
using RequestDelegate = Microsoft.AspNetCore.Http.RequestDelegate;
using Demo.Types;
using HotChocolate.AspNetCore;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Demo.Middleware;

public class SchemaSelectionMiddleware
{
    string introspectionQuery = @"
    query IntrospectionQuery {
        __schema {
            queryType { name }
            mutationType { name }
            subscriptionType { name }
            types {
                kind
                name
                description
                fields(includeDeprecated: true) {
                    name
                    description
                    args {
                        name
                        description
                        type {
                            name
                            kind
                            ofType {
                                name
                                kind
                            }
                        }
                        defaultValue
                    }
                    type {
                        kind
                        name
                        ofType {
                            name
                            kind
                            ofType {
                                name
                                kind
                                ofType {
                                    name
                                    kind
                                    ofType {
                                        name
                                        kind
                                    }
                                }
                            }
                        }
                    }
                    isDeprecated
                    deprecationReason
                }
                inputFields {
                    name
                    description
                    type {
                        name
                        kind
                        ofType {
                            name
                            kind
                        }
                    }
                    defaultValue
                }
                interfaces {
                    name
                    kind
                    ofType {
                        name
                        kind
                    }
                }
                enumValues(includeDeprecated: true) {
                    name
                    description
                    isDeprecated
                    deprecationReason
                }
                possibleTypes {
                    name
                    kind
                    ofType {
                        name
                        kind
                    }
                }
            }
            directives {
                name
                description
                locations
                args {
                    name
                    description
                    type {
                        name
                        kind
                        ofType {
                            name
                            kind
                        }
                    }
                    defaultValue
                }
            }
        }
    }";

    private readonly RequestDelegate _next;
  //  private readonly MiddlewareRoutingType _routing;
    private GraphQLServerOptions? _options;

    public SchemaSelectionMiddleware(RequestDelegate next)
    {
        _next = next;
      //  _routing = routing;
    }

    public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
    {
        context.Request.EnableBuffering();

        // Buffer to store the read characters
        var buffer = new char[200];
        string firstFiftyChars;

        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8,
                 detectEncodingFromByteOrderMarks: false, bufferSize: 1024,
                 leaveOpen: true))
        {
            // Read up to 100 characters from the request body
            int charCount = await reader.ReadAsync(buffer, 0, buffer.Length);

            // Create a string from the read characters
            firstFiftyChars = new string(buffer, 0, charCount);

            // Reset the stream position to zero to allow further reading downstream
            context.Request.Body.Position = 0;
        }
        // Convert char array to string
        // string firstFiftyChars = new string(buffer);

        if (firstFiftyChars.Length == 0 || firstFiftyChars.IndexOf("{\"operationName\":\"IntrospectionQuery\"") == -1)
        {
            await _next(context);
            return;
        }

        // Dictionary<string, string> dicPattern = new Dictionary<string, string>();
        //dicPattern.Add("Response\\__TypeSchemaResponse.json", "{\"operationName\":\"IntrospectionQuery\".*__type\\(name:.*__Schema");
        // dicPattern.Add("Response\\__DirectiveResponse.json", "{\"operationName\":\"IntrospectionQuery\".*__type\\(name:.*__Directive");
        // dicPattern.Add("Response\\__InputValueResponse.json", "{\"operationName\":\"IntrospectionQuery\".*__type\\(name:.*__InputValue");
        // dicPattern.Add("Response\\__typeResponse.json", "{\"operationName\":\"IntrospectionQuery\".*__type\\(name:.*__Type");
        // dicPattern.Add("Response\\__SchemaResponse.json", "\"operationName\"\\s*:\\s*\"IntrospectionQuery\"\\s*,\\s*\"query\"\\s*:\\s*\"query\\s*IntrospectionQuery\\s*\\{.*__schema");

         Regex regex = new Regex("\"operationName\"\\s*:\\s*\"IntrospectionQuery\"\\s*,\\s*\"query\"\\s*:\\s*\"query\\s*IntrospectionQuery\\s*\\{.*__schema");
         if(!regex.IsMatch(firstFiftyChars))
         {
             await _next(context);
             return;
         }

        var schemaBuilder = SchemaBuilder.New();
        schemaBuilder.AddQueryType<QueryType>();
        schemaBuilder.AddType<AccidentInfoType>();
     
        schemaBuilder.AddType(new AccidentPartyType(context.User));


        // schemaBuilder.AddMiddleware<AuthorizationMiddleware>(); // Apply authorization middleware

        // schemaBuilder.AddMiddleware<AuthorizationMiddleware>(); // Ensure middleware is added to schema
        var schema = schemaBuilder.Create();
        var executor = schema.MakeExecutable();
        //var introspectionQuery = IntrospectionQuery.Default; // Default GraphQL introspection query
        var result = await executor.ExecuteAsync(introspectionQuery);
        var jsonResponse = result.ToJson();

        // Set the response content type to application/json
        context.Response.ContentType = "application/json";

        // Write the JSON response to the HttpContext.Response
        await context.Response.WriteAsync(jsonResponse);
        return;
        // return json;

       // await _next(context);
       

    }

    private async Task<string> ReadRequestBody(HttpRequest request)
    {
        using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
        return await reader.ReadToEndAsync();
    }

    private async Task LogRequestBody(string fileName, string body)
    {
        await File.AppendAllTextAsync(fileName, body);
    }

    private async Task LogResponseBody(string fileName, string body)
    {
        await File.AppendAllTextAsync(fileName, body);
    }

    private GraphQLServerOptions GetOptions(HttpContext context)
    {
        if (_options is not null)
        {
            return _options;
        }     

        _options = GetGraphQLServerOptions(context);
        return _options;
    }

    public GraphQLServerOptions? GetGraphQLServerOptions(HttpContext context)
      => context.GetEndpoint()?.Metadata.GetMetadata<GraphQLServerOptions>() ??
         (context.Items.TryGetValue(nameof(GraphQLServerOptions), out var o) &&
          o is GraphQLServerOptions options
              ? options
              : null);
}
