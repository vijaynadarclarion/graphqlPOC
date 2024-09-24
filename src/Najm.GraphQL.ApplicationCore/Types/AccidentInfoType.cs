

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using HotChocolate.Execution;
using HotChocolate.Types;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Services;
using Najm.GraphQL.ApplicationCore.Entity;


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

       // descriptor.Field("accidentParties")
          // .ResolveWith<AccidentService>(p => p.GetAccidentPartiesAsync(default!));
        

      
    }
}

