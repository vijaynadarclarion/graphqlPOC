using System;
using System.Runtime.Serialization;
using HotChocolate;

namespace Najm.GraphQL.Infrastructure.Attribute;
[Serializable]
internal class GraphQLRequestException : Exception
{
    private IError _error;

    public GraphQLRequestException()
    {
    }

    public GraphQLRequestException(IError error)
    {
        _error = error;
    }

    public GraphQLRequestException(string message) : base(message)
    {
    }

    public GraphQLRequestException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected GraphQLRequestException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}