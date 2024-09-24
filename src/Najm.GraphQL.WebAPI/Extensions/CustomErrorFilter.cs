using HotChocolate.AspNetCore;
using HotChocolate;
using System.Linq;
using Serilog;

namespace Najm.GraphQL.WebAPI.Extensions;

public class CustomErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        Log.Logger.Error("{0}:{1}", error.Message, error.Exception);

        if (error.Exception is GraphQLRequestException gqlException)
        {           
            return gqlException.Errors.FirstOrDefault();
        }


        return error;
    }
}
