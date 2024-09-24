

using HotChocolate.Types;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Entity;

namespace Najm.GraphQL.ApplicationCore.Types;

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

     //   descriptor.Field("accidentParties")
         //   .ResolveWith<IAccidentService>(r => 
          //      r.GetAccidentPartiesAsync(default!, default!));
        //.Type<IEnumerable<AccidentParty>>();
        //.UsePaging<NonNullType<AccidentPartyType>>();
        //.UseScopedDbContext<AppReadOnlyDbContext>();
        //.UseDbContext<AppReadOnlyDbContext>();


      
    }
}

