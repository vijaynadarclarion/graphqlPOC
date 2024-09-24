using HotChocolate.Resolvers;

namespace Demo.Middleware
{

    public class AuthorizationMiddleware 
    {
        private readonly FieldDelegate _next;

        public AuthorizationMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context, FieldDelegate next)
        {
            var user = context.GetUser();
            if (user == null || user.HasClaim("Permission", "CanViewSensitiveData") == false)
            {
                context.Result = null; // Setting the result to null
                return;
            }

            await next(context);
        }

        
    }

}
