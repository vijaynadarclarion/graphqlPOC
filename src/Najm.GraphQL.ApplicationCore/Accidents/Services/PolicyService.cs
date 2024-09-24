using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Specifications;
using Najm.GraphQL.ApplicationCore.Entity;
using Najm.GraphQL.ApplicationCore.Interfaces;
using Polly;

namespace Najm.GraphQL.ApplicationCore.Accidents.Services;

public class PolicyService : IPolicyService
{
    private readonly IPolicyResolver _policyResolver;

    public PolicyService(IPolicyResolver policyResolver)
    {
        _policyResolver = policyResolver;
    }

    public async Task<List<VehicleDetail>> GetPolicies([Parent] AccidentParty accidentPartyInfo)
    {
        if (accidentPartyInfo == null)
            throw new ArgumentNullException(nameof(accidentPartyInfo));

        if (accidentPartyInfo.VehicleId == null)
            return null;

        if (accidentPartyInfo.VehicleId <= 0)
            return null;

        return await _policyResolver.GetPolicies(accidentPartyInfo.VehicleId.Value);
    }
}
