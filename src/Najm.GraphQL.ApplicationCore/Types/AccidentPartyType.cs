using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;

using HotChocolate.Authorization;
using HotChocolate.Types;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Dtos;
using Najm.GraphQL.ApplicationCore.Accidents.GraphQLExtensions;
using Najm.GraphQL.ApplicationCore.Accidents.Services;
using Najm.GraphQL.ApplicationCore.Entity;

namespace Najm.GraphQL.Infrastructure.Types;

public class AccidentPartyType : ObjectType<AccidentParty>
{
    private readonly ClaimsPrincipal _claimsPrincipal;
    private string fullyQualifiedName;
    private readonly AuthorizationConfig _config;
    private readonly bool _schemaDownLoad;
    public AccidentPartyType(bool schemaDownLoad, ClaimsPrincipal claimsPrincipal, AuthorizationConfig config)
    {
        _claimsPrincipal = claimsPrincipal;
        fullyQualifiedName = typeof(AccidentParty).Name;
        _config = config;
        _schemaDownLoad = schemaDownLoad;
    }

    protected override void Configure(IObjectTypeDescriptor<AccidentParty> descriptor)
    {
        /*descriptor.Field(a => a.CaseInfoId);
        descriptor.Field(a => a.CasepartyId);
        descriptor.Field(a => a.IsInsured);
        descriptor.Field(a => a.InsurancePolicyNo)            
            .IgnoreField(fullyQualifiedName, _claimsPrincipal)
            .Use<DynamicAuthorizeMiddleware>();
        */

        string fullPolicesName = string.Format("{0}.{1}", fullyQualifiedName, "policies").ToLower();

       if (_config != null && _config.Authorization.Any(x => x.Name == fullPolicesName))
        {
            descriptor.Field("policies")                
                .ResolveWith<PolicyService>(r => r.GetPolicies(default!))
                .UseFirstOrDefault()
                .IgnoreField(fullyQualifiedName, _claimsPrincipal, _schemaDownLoad)
                .Use<DynamicAuthorizeMiddleware>();
        }
        else
        {
            descriptor.Field("policies")                
               .ResolveWith<PolicyService>(r => r.GetPolicies(default!))
               .UseFirstOrDefault();

              
        }

   

        var properties = typeof(AccidentParty).GetProperties();
       foreach (var property in properties)
        {
            string fullName = string.Format("{0}.{1}", fullyQualifiedName, property.Name).ToLower();

            if (_config != null && _config.Authorization.Any(x => x.Name == fullName))
            {

                var propertyAccessExpression = CreatePropertyAccessExpression(property);
                descriptor.Field(propertyAccessExpression)
                         .Type(GetGraphQLType(property.PropertyType))
                         .IgnoreField(fullyQualifiedName, _claimsPrincipal, _schemaDownLoad)
                         .Use<DynamicAuthorizeMiddleware>();
            }
            else
            {
                var propertyAccessExpression = CreatePropertyAccessExpression(property);
                descriptor.Field(propertyAccessExpression)
                        .Type(GetGraphQLType(property.PropertyType));
            }

       

        }


    }

    private Expression<Func<AccidentParty, object>> CreatePropertyAccessExpression(PropertyInfo propertyInfo)
    {
        var param = Expression.Parameter(typeof(AccidentParty), "x");
        var propertyAccess = Expression.Property(param, propertyInfo);
        var convert = Expression.Convert(propertyAccess, typeof(object)); // Convert to object to match Func<AccidentParty, object>
        return Expression.Lambda<Func<AccidentParty, object>>(convert, param);
    }

    private Type GetGraphQLType(Type propertyType)
    {
        // Map .NET types to GraphQL types
        if (propertyType == typeof(int))
            return typeof(IntType);
        if (propertyType == typeof(string))
            return typeof(StringType);

        return typeof(StringType);
    }



}
