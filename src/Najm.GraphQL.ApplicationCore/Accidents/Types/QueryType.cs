using System.Security.Claims;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Services;
using Najm.GraphQL.ApplicationCore.Entity;


namespace Najm.GraphQL.ApplicationCore.Types
{
    public class QueryType : ObjectType<Query>
    {
       /* private readonly IAccidentService _accidentService;
        public QueryType(IAccidentService accidentService)
        {
            _accidentService = accidentService;
        }*/

        protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
        {
            descriptor.Description("Represents any insurance company information.");

            descriptor
                .Field("accidents")
                .Argument("caseNumbers", argumentDescriptor =>
                     argumentDescriptor.Type<ListType<StringType>>())                
                .ResolveWith<IAccidentResolver>(p => p.GetAccident(default!))
                //.UseScopedDbContext<AppReadOnlyDbContext>()
                //.UseDbContext< AppReadOnlyDbContext>()
                .Description("Represents the accident information.");         

        }
    }
}
