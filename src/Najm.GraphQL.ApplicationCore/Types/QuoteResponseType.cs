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
using Najm.GraphQL.ApplicationCore.Types;

namespace Najm.GraphQL.Infrastructure.Types;

public class QuoteResponseType : ObjectType<QuoteResponse>
{
    private readonly ClaimsPrincipal _claimsPrincipal;
    private string fullyQualifiedName;
    private readonly AuthorizationConfig _config;
    private readonly bool _schemaDownLoad;
    public QuoteResponseType(bool schemaDownLoad, ClaimsPrincipal claimsPrincipal, AuthorizationConfig config)
    {
        _claimsPrincipal = claimsPrincipal;
        fullyQualifiedName = typeof(QuoteResponse).Name;
        _config = config;
        _schemaDownLoad = schemaDownLoad;
    }

    protected override void Configure(IObjectTypeDescriptor<QuoteResponse> descriptor)
    {

       /* var properties = typeof(QuoteResponse).GetProperties();
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

       

        }*/


    }

    private Expression<Func<QuoteResponse, object>> CreatePropertyAccessExpression(PropertyInfo propertyInfo)
    {
        var param = Expression.Parameter(typeof(QuoteResponse), "x");
        var propertyAccess = Expression.Property(param, propertyInfo);
        var convert = Expression.Convert(propertyAccess, typeof(object)); // Convert to object to match Func<AccidentParty, object>
        return Expression.Lambda<Func<QuoteResponse, object>>(convert, param);
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
