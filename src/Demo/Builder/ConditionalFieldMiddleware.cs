using HotChocolate;
using System.Linq;
using HotChocolate.Configuration;
using HotChocolate.Language;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using HotChocolate.AspNetCore;

namespace Demo
{

    public class ConditionalFieldMiddleware
    {
        private readonly FieldDelegate _next;

        public ConditionalFieldMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
           // var httpContext = context.GetHttpContext();
            var requestPath = context.Path;

            // Check if the request URL matches a specific path
            /*if (requestPath.StartsWithSegments("/graphql/admin"))
            {
                // Apply specific logic for admin requests
                context.Result = "Admin request";
            }*/

            // Continue to the next middleware or resolver
            await _next(context);
        }
    }


}
