using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using System.Security.Claims;

namespace Demo.Interpretors;

public class HttpRequestInterceptor : DefaultHttpRequestInterceptor
{
    public override ValueTask OnCreateAsync(HttpContext context,
        IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
      var identity = new ClaimsIdentity();
        identity.AddClaim(new Claim(ClaimTypes.Country, "us"));
        identity.AddClaim(new Claim(ClaimTypes.Name, "vijay"));
        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, "1"));
        identity.AddClaim(new Claim(ClaimTypes.Role, "Administrator"));
        identity.AddClaim(new Claim("Permission", "CanViewSensitiveData"));

        context.User.AddIdentity(identity);
       
        return base.OnCreateAsync(context, requestExecutor, requestBuilder,
            cancellationToken);
    }
}

