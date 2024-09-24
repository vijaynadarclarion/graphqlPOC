

using HotChocolate.Types;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Entity;
using Najm.GraphQL.Infrastructure.Accidents.Resolvers;

namespace Najm.GraphQL.Infrastructure.Types;

public class AccidentInfoType : ObjectType<AccidentInfo>
{  

    protected override void Configure(IObjectTypeDescriptor<AccidentInfo> descriptor)
    {
        descriptor.Description("Represents accident information.");

        descriptor
            .Field(p => p.CaseInfoId)
            .Description("Accident Id.");

        descriptor
        .Field(p => p.CaseNumber)
        .Description("Accident case number.");

        descriptor
        .Field(p => p.CaseRegisterationTime)
        .Description("Accident case registration time.");

       descriptor.Field("accidentParties")
           .ResolveWith<AccidentResolver>(r => 
               r.GetAccidentPartiesAsync(default!));
        //.Type<IEnumerable<AccidentParty>>();
        //.UsePaging<NonNullType<AccidentPartyType>>();
        //.UseScopedDbContext<AppReadOnlyDbContext>();
        //.UseDbContext<AppReadOnlyDbContext>();


      
    }
}

