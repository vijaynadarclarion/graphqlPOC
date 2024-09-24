using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using HotChocolate.Resolvers;
using System.Security.Claims;
using AutoMapper.Execution;
using HotChocolate.Data.Projections.Context;
using Polly;

namespace Najm.GraphQL.ApplicationCore.Accidents.GraphQLExtensions;



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
        var user = context.GetUser();
        var field = context.GetSelectedField();        
        string name = field.Selection.DeclaringType.Name.ToLower() + "." + field.Selection.Field.Name.ToLower();
        var claims = user?.Claims;

        bool hasClaim = claims.Any(x =>
         (x.Type.ToLower() == name && x.Value.ToLower() == "true") ||
         (x.Type == ClaimTypes.Role && x.Value.ToLower() == "admin"));

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








