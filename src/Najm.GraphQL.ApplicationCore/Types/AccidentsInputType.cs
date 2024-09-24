using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using HotChocolate.Types;
using Najm.GraphQL.ApplicationCore.Accidents.Dtos;
using Najm.GraphQL.ApplicationCore.Accidents.GraphQLExtensions;
using Najm.GraphQL.ApplicationCore.Entity;
using Najm.GraphQL.ApplicationCore.Accidents.GraphQLExtensions;
using HotChocolate.Authorization;
namespace Najm.GraphQL.ApplicationCore.Types;
public class AccidentsInput
{
    public List<string> CaseNumbers { get; set; }
    public string AccidentLocation { get; set; }
    public DateTime? AccidentDate { get; set; }
    public string ReportedBy { get; set; }
}


public class AccidentsInputType : InputObjectType<AccidentsInput>
{

    private readonly ClaimsPrincipal _claimsPrincipal;
    private string fullyQualifiedName;
    private readonly AuthorizationConfig _config;
    private readonly bool _schemaDownLoad;
    public AccidentsInputType(bool schemaDownLoad, ClaimsPrincipal claimsPrincipal, AuthorizationConfig config)
    {
        _claimsPrincipal = claimsPrincipal;
        fullyQualifiedName = typeof(AccidentsInput).Name;
        _config = config;
        _schemaDownLoad = schemaDownLoad;
    }

    protected override void Configure(IInputObjectTypeDescriptor<AccidentsInput> descriptor)
    {

        var properties = typeof(AccidentsInput).GetProperties();
        foreach (var property in properties)
        {
            string fullName = string.Format("{0}.{1}", fullyQualifiedName, property.Name).ToLower();

            if (_config != null && _config.Authorization.Any(x => x.Name == fullName))
            {

                var propertyAccessExpression = CreatePropertyAccessExpression(property);
                descriptor.Field(propertyAccessExpression)
                         .Type(GetGraphQLType(property.PropertyType))
                         .IgnoreField(fullyQualifiedName, _claimsPrincipal, _schemaDownLoad);


                //.Use<DynamicAuthorizeMiddleware>();
            }
            else
            {
                var propertyAccessExpression = CreatePropertyAccessExpression(property);
                descriptor.Field(propertyAccessExpression)
                        .Type(GetGraphQLType(property.PropertyType));
            }


        }

        // descriptor.Field(f => f.AccidentDate).Ignore();
        //


    }

    private Expression<Func<AccidentsInput, object>> CreatePropertyAccessExpression(PropertyInfo propertyInfo)
    {
        var param = Expression.Parameter(typeof(AccidentsInput), "x");
        var propertyAccess = Expression.Property(param, propertyInfo);
        var convert = Expression.Convert(propertyAccess, typeof(object)); // Convert to object to match Func<AccidentParty, object>
        return Expression.Lambda<Func<AccidentsInput, object>>(convert, param);
    }

    private Type GetGraphQLType(Type propertyType)
    {
        // Map .NET types to GraphQL types
        if (propertyType == typeof(int))
            return typeof(IntType);
        if (propertyType == typeof(string))
            return typeof(StringType);
        if (propertyType == typeof(List<string>))
            return typeof(ListType<StringType>);
        return typeof(StringType);
    }



}
