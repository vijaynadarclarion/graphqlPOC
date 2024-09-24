using Demo.Data;
using Demo.Entity;
using HotChocolate.Execution;

namespace Demo.Resolvers;

public class TestResolver
{
    public AccidentInfo GetAccident(List<string> caseNumbers)
    {
        return new AccidentInfo();
    }
}
    public class AccidentResolver
{
    private readonly IDbContextFactory<AppReadOnlyDbContext> _dbContextFactory;

    public AccidentResolver(
            IDbContextFactory<AppReadOnlyDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory ??
            throw new ArgumentNullException(nameof(dbContextFactory));
    }

    public IQueryable<AccidentInfo> GetAccident([ScopedService] AppReadOnlyDbContext context, List<string> caseNumbers)
    {
        if(caseNumbers == null)
            throw new ArgumentNullException(nameof(caseNumbers));

        return context.CaseInfos.Where(x => caseNumbers.Contains(x.CaseNumber));
       
    }

    public IQueryable<AccidentParty> GetAccidentPartiesAsync([Parent] AccidentInfo accidentInfo,
                                                                          [ScopedService] AppReadOnlyDbContext dbContext)
    {        
        return dbContext.CaseParties.Where(a => a.CaseInfoId == accidentInfo.CaseInfoId);
    }

   
}       
        
