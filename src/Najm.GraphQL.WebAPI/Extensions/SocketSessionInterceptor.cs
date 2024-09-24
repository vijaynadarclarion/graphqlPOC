using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;
using System;
using HotChocolate.AspNetCore.Subscriptions;
using HotChocolate.AspNetCore.Subscriptions.Protocols;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate.AspNetCore.Subscriptions.Messages;
using System.Collections.Generic;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Polly;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Najm.GraphQL.WebAPI.Extensions;

public class HeaderWebSocket
{
    public string accept { get; set; }
    public string Authorization { get; set; }

}

public class SocketSessionInterceptor : DefaultSocketSessionInterceptor
{
    private readonly TokenValidationParameters _tokenValidationParameters;


    public SocketSessionInterceptor(TokenValidationParameters tokenValidationParameters)
    {
        _tokenValidationParameters = tokenValidationParameters;
    }


    public override async ValueTask<ConnectionStatus> OnConnectAsync(
        ISocketSession session,
        IOperationMessagePayload connectionInitMessage,
        CancellationToken cancellationToken = default)
    {
        string token = null;
        try
        {
            // Scenario 1: Token from WebSocket headers
            var httpContext = session.Connection.HttpContext;
            if (httpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
            {
                token = authorizationHeader.ToString().Replace("Bearer ", ""); // Remove "Bearer " if present
            }

            // Scenario 2: Token in connectionInit payload
            // Extract the Authorization from the connectionInit payload
            if (connectionInitMessage.ToPropertyDictionary().TryGetValue("Payload", out var payload))
            {
                // Deserialize the payload using System.Text.Json to a JsonElement
                JsonElement jsonPayload = (JsonElement)payload;

                // Check if it has an "Authorization" field
                if (jsonPayload.TryGetProperty("Authorization", out JsonElement authTokenElement))
                {
                    token = authTokenElement.GetString()?.Replace("Bearer ", "").Trim();
                }
            }

            if (string.IsNullOrEmpty(token))
            {
                // If token is missing, return a failure status
                return ConnectionStatus.Reject("Missing or invalid Authorization token.");
            }

            // Token validation logic
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out SecurityToken validatedToken);

            // If token is valid, you can proceed with the connection
            if (validatedToken != null)
            {
                // Set the user context to the session
                session.Connection.HttpContext.User = principal;
                ConnectionStatus.Accept();
                // Proceed with the base connection logic
                return await base.OnConnectAsync(session, connectionInitMessage, cancellationToken);
            }

            return ConnectionStatus.Reject("Invalid token.");
        }
        catch (Exception ex)
        {
            // Log or handle the exception if necessary
            return ConnectionStatus.Reject($"Token validation failed: {ex.Message}");
        }
    }


    public override ValueTask OnCloseAsync(
       ISocketSession connection,
       CancellationToken cancellationToken)
    {
        // This method is called when the WebSocket connection is closed
        Console.WriteLine("WebSocket connection closed.");
        return base.OnCloseAsync(connection, cancellationToken);
    }

    // This method will be called when a subscription is initialized
    public override ValueTask OnRequestAsync(
        ISocketSession connection,
        string operationSessionId,
       IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        // Inspect the incoming subscription request message here

        var query = requestBuilder.Create().Query;

        // Here, you can inspect the subscription query
        Console.WriteLine("Incoming Subscription Query:");
        Console.WriteLine(query);

        // Example: You can inspect the arguments if needed
        if (requestBuilder.Create().VariableValues != null)
        {
            foreach (var variable in requestBuilder.Create().VariableValues)
            {
                Console.WriteLine($"{variable.Key}: {variable.Value}");
            }
        }

        return base.OnRequestAsync(connection, operationSessionId, requestBuilder, cancellationToken);
    }



}

