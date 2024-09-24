using System.Reflection;
using HotChocolate.Resolvers;


namespace Demo.Query;

public class DynamicAuthorizeMiddleware
{
    private readonly FieldDelegate _next;

    private readonly MemberInfo _memberInfo;

    public DynamicAuthorizeMiddleware(FieldDelegate next, MemberInfo memberInfo)
    {
        _next = next;
        _memberInfo = memberInfo;
    }

    // this method must be called InvokeAsync or Invoke
    public async Task InvokeAsync(IMiddlewareContext context)
    {       
    

        bool hasClaim = false;

        // Check if the user has the required permission
        if (hasClaim)
        {
            await _next(context);
        }
        else
        {
            // Creating the error using HotChocolate's error builder
            throw new GraphQLException(
                ErrorBuilder.New()
                .SetMessage("Unauthorized access.")
                .SetCode("AUTH_NOT_AUTHORIZED")
                .SetPath(context.Path)
                .Build());

            /* throw new GraphQLRequestException(ErrorBuilder.New()
                 .SetMessage("You do not have permission to access this field.")
                 .SetCode("AUTH_NOT_AUTHORIZED")
                 .Build());*/
        }

        
    }
}








