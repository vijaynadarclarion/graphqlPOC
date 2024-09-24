using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HotChocolate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Najm.GraphQL.ApplicationCore._Interfaces;
using Najm.GraphQL.ApplicationCore.Accidents.Specifications;
using Najm.GraphQL.ApplicationCore.Entity;
using Najm.GraphQL.ApplicationCore.Interfaces;
using Najm.GraphQL.Infrastructure.Data;
using Polly;

namespace Najm.GraphQL.Infrastructure.Accidents.Resolvers;

public class AccidentResolver : IAccidentResolver
{

    private readonly AppReadOnlyDbContext _dbContext;
   // private readonly IDbContextFactory<AppReadOnlyDbContext> _dbContextFactory;
    public AccidentResolver(IDbContextFactory<AppReadOnlyDbContext> dbContextFactory)
    {
          _dbContext = dbContextFactory.CreateDbContext();
        //_dbContextFactory = dbContextFactory;
    }

    public ValueTask DisposeAsync()
    {
       return _dbContext.DisposeAsync();
    }  

    public IQueryable<AccidentInfo> GetAccident(List<string> caseNumbers)
    {
       
        if (caseNumbers == null)
            throw new ArgumentNullException(nameof(caseNumbers));

        return _dbContext.CaseInfos.Where(x => caseNumbers.Contains(x.CaseNumber));
       
    }

    public IQueryable<AccidentParty> GetAccidentPartiesAsync([Parent] AccidentInfo accidentInfo)
    {
        return _dbContext.CaseParties.Where(a => a.CaseInfoId == accidentInfo.CaseInfoId);
    }
}
