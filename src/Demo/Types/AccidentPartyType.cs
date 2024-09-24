﻿using System.Security.Claims;
using Demo.Entity;
using Demo.Middleware;
using Demo.Resolvers;
using HotChocolate.Authorization;
using HotChocolate.Types;

namespace Demo.Types;

public class AccidentPartyType : ObjectType<AccidentParty>
{
    private readonly ClaimsPrincipal _claimsPrincipal;

    public AccidentPartyType(ClaimsPrincipal claimsPrincipal)
    {
        _claimsPrincipal = claimsPrincipal;
    }

    protected override void Configure(IObjectTypeDescriptor<AccidentParty> descriptor)
    {
        descriptor.Field(a => a.CaseInfoId).Type<IdType>();
        descriptor.Field(a => a.CasepartyId).Type<IdType>();
        descriptor.Field(a => a.IsInsured).Type<IdType>();
        descriptor.Field(a => a.InsurancePolicyNo).Type<IdType>();

        // var user = _httpContextAccessor.HttpContext?.User;
         var hasPermission = _claimsPrincipal?.Claims.Any(c => c.Type == "client_id"
        && c.Value == "mvc") ?? false;

        if (hasPermission)
        {
            descriptor.Field("policies")
                .ResolveWith<PolicyResolver>(r => r.GetPolicies(default!, default!))                
                .UseFirstOrDefault();
              
              //  .Use<AuthorizationMiddleware>()
              //  .Extend().OnBeforeCreate((ctx, def) =>
                  //  def.ContextData["Permission"] = "CanViewSensitiveData");
            // .Directive(new AuthorizeDirective("CanAccessSensitiveData"))
            // .UseFirstOrDefault(); // Allow null return value;
        }
    }


}
