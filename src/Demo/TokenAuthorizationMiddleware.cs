using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace Demo;

public class TokenAuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public TokenAuthorizationMiddleware(RequestDelegate next, TokenValidationParameters tokenValidationParameters)
    {
        _next = next;
        _tokenValidationParameters = tokenValidationParameters;
    }


    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/playground"))
        {
            // Skip authorization for /playground path
            await _next(context);
            return;
        }

        var authorizationHeader = context.Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }

        var token = authorizationHeader.Substring("Bearer ".Length).Trim();

        try
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(token))
            {
                throw new SecurityTokenException("Invalid token format");
            }

            var principal = jwtHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);

            if (!IsTokenValid(validatedToken))
            {
                throw new SecurityTokenException("Invalid token");
            }

            // Attach the user principal to the HttpContext
            context.User = principal;

            await _next(context);
        }
        catch (Exception)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
        }
    }

    private bool IsTokenValid(SecurityToken validatedToken)
    {
        if (!(validatedToken is JwtSecurityToken jwtToken))
        {
            return false;
        }

        // Additional validation logic can be added here, for example:
        // - Check issuer
        // - Check audience
        // - Check custom claims

        return true;
    }
}
