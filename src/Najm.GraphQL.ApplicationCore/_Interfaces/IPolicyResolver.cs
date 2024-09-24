using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using Najm.GraphQL.ApplicationCore.Entity;
using Najm.GraphQL.ApplicationCore.Interfaces;

namespace Najm.GraphQL.ApplicationCore._Interfaces;
public interface IPolicyResolver
{
    Task<List<VehicleDetail>> GetPolicies(decimal vehicleId);
}


public interface IPolicyService
{
    Task<List<VehicleDetail>> GetPolicies([Parent] AccidentParty accidentPartyInfo);
}
