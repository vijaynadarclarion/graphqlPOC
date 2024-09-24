using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotChocolate.Types;

namespace Najm.GraphQL.ApplicationCore.Accidents.GraphQLExtensions;
public static class ObjectFieldDescriptorExtension
{
    public static IObjectFieldDescriptor IgnoreField(
            this IObjectFieldDescriptor descriptor,
            string fullyQualifiedName,
            ClaimsPrincipal claimsPrincipal, bool schemaDownLoad)

    {
        var fieldDefinition = descriptor.Extend().Definition;

        // Construct the full name, typically TypeName.FieldName
        string fieldName = fieldDefinition.Name;
        // string typeName = fieldDefinition.Member?.DeclaringType?.Name ?? "UnknownType";
        string fullFieldName = $"{fullyQualifiedName}.{fieldName}".ToLower();

        if (!schemaDownLoad)
            return descriptor;

        if (claimsPrincipal == null)
        {
            descriptor.Ignore();
            return descriptor;
        }

        bool hasClaim = claimsPrincipal.Claims.Any(x =>
            (x.Type.ToLower() == fullFieldName && x.Value.ToLower() == "true") ||
            (x.Type == ClaimTypes.Role && x.Value.ToLower() == "admin"));

        if (!hasClaim)
        {
            descriptor.Ignore();
        }

        return descriptor;
    }


    public static IInputFieldDescriptor IgnoreField(
           this IInputFieldDescriptor descriptor,
           string fullyQualifiedName,
           ClaimsPrincipal claimsPrincipal, bool schemaDownLoad)

    {
        var fieldDefinition = descriptor.Extend().Definition;

        // Construct the full name, typically TypeName.FieldName
        string fieldName = fieldDefinition.Name;
        // string typeName = fieldDefinition.Member?.DeclaringType?.Name ?? "UnknownType";
        string fullFieldName = $"{fullyQualifiedName}.{fieldName}".ToLower();

        if (!schemaDownLoad)
            return descriptor;

        if (claimsPrincipal == null)
        {
            descriptor.Ignore();
            return descriptor;
        }

        bool hasClaim = claimsPrincipal.Claims.Any(x =>
            (x.Type.ToLower() == fullFieldName && x.Value.ToLower() == "true") ||
            (x.Type == ClaimTypes.Role && x.Value.ToLower() == "admin"));

        if (!hasClaim)
        {
            descriptor.Ignore();
        }

        return descriptor;
    }

}
