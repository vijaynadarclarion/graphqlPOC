using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HotChocolate;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Specifications;
using Najm.GraphQL.ApplicationCore.Entity;
using Najm.GraphQL.ApplicationCore.Interfaces;
using Najm.GraphQL.ApplicationCore.Types;
using Polly;

namespace Najm.GraphQL.ApplicationCore.Accidents.Services;

public class AccidentService : IAccidentService
{
   // private readonly IReadOnlyRepository<AccidentInfo> _accident_ReadRepo;
   // private readonly IReadOnlyRepository<AccidentParty> _accidentParty_ReadRepo;
    private readonly IHttpContextAccessor _httpContextAccessor;

   /* public AccidentService(IReadOnlyRepository<AccidentInfo> accident_ReadRepo,
        IReadOnlyRepository<AccidentParty> accidentParty_ReadRepo, IHttpContextAccessor contextAccessor)
    {
        _accident_ReadRepo = accident_ReadRepo;
        _accidentParty_ReadRepo = accidentParty_ReadRepo;
        _httpContextAccessor = contextAccessor;
    }*/

    public AccidentService(IHttpContextAccessor contextAccessor)
    {
       
        _httpContextAccessor = contextAccessor;
    }

    public async Task<List<AccidentInfo>> GetAccident(AccidentsInput input)
    {
        // if (caseNumbers == null)
        //  throw new ArgumentNullException(nameof(caseNumbers));

       
       
        return new List<AccidentInfo>();

    }


}
