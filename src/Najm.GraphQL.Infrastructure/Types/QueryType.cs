using System.Security.Claims;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Dtos;
using Najm.GraphQL.ApplicationCore.Accidents.Services;
using Najm.GraphQL.ApplicationCore.Entity;
using Najm.GraphQL.Infrastructure.Accidents.Resolvers;


namespace Najm.GraphQL.Infrastructure.Types
{
    public class QueryType : ObjectType<Query>
    {
        private readonly AuthorizationConfig _config;
        public QueryType(AuthorizationConfig config)
        {
            _config = config;
        }

        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Description("Represents any insurance company information.");

            descriptor
                .Field("accidents")
                .Argument("caseNumbers", argumentDescriptor =>
                     argumentDescriptor.Type<ListType<StringType>>())                
                .ResolveWith<AccidentResolver>(p => p.GetAccident(default!))
                //.UseScopedDbContext<AppReadOnlyDbContext>()
                //.UseDbContext< AppReadOnlyDbContext>()
                .Description("Represents the accident information.");         

        }
    }
}
