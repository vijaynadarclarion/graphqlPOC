using System.Collections.Generic;
using System.Security.Claims;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Dtos;
using Najm.GraphQL.ApplicationCore.Accidents.Services;
using Najm.GraphQL.ApplicationCore.Entity;
using Najm.GraphQL.ApplicationCore.Types;


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
                     //.Argument("input", a => a.Type<NonNullType<InputObjectType<AccidentsInput>>>())
                     .Argument("input", a => a.Type<NonNullType<AccidentsInputType>>()) // Register the input type here

                    .ResolveWith<AccidentService>(p => p.GetAccident(default!))
                    .Description("Represents the accident information.");        

        }
    }
}
