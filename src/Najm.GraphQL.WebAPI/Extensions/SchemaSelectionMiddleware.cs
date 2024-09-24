using HotChocolate.AspNetCore;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;
using System;
using Najm.GraphQL.Infrastructure.Types;
using HotChocolate.Execution;
using RequestDelegate = Microsoft.AspNetCore.Http.RequestDelegate;
using Najm.GraphQL.ApplicationCore.Accidents.Dtos;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.Types;
using Najm.GraphQL.ApplicationCore.Types;
using Najm.GraphQL.ApplicationCore.Entity;
using Microsoft.AspNetCore.Authorization;

namespace Najm.GraphQL.WebAPI.Extensions;

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
        
        bool interoSpectionQuery = ((firstFiftyChars.IndexOf("query IntrospectionQuery") != -1 && firstFiftyChars.IndexOf("__schema") != -1));      
      
       if (firstFiftyChars.Length == 0 || !interoSpectionQuery)
        {
            await _next(context);
            return;
        }

        AuthorizationConfig authorizationConfig = context.RequestServices.GetService<AuthorizationConfig>();
        var schemaBuilder = SchemaBuilder.New();
        schemaBuilder.AddQueryType<QueryType>();
        schemaBuilder.AddType<AccidentInfoType>();
        schemaBuilder.AddType(new AccidentPartyType(true, context.User, authorizationConfig));
        schemaBuilder.AddType(new AccidentsInputType(true, context.User, authorizationConfig));
        //schemaBuilder.AddInputObjectType<AccidentsInputType>();
        schemaBuilder.AddType(new QuoteResponseType(true, context.User, authorizationConfig));
       // schemaBuilder.AddSubscriptionType<Subscription>();


        try
        {
          var schema = schemaBuilder.Create();
          var executor = schema.MakeExecutable();
          var result = await executor.ExecuteAsync(introspectionQuery);
          var jsonResponse = result.ToJson();

          // Set the response content type to application/json
          context.Response.ContentType = "application/json";

          // Write the JSON response to the HttpContext.Response
          await context.Response.WriteAsync(jsonResponse);
          return;
      }
      catch(Exception ex)
      {
          string message = ex.Message;
          throw;
      }

    }
}

   


