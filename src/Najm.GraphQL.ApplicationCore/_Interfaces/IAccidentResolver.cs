using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotChocolate;
using Najm.GraphQL.ApplicationCore.Entity;
using Najm.GraphQL.ApplicationCore.Interfaces;
using Najm.GraphQL.ApplicationCore.Types;

namespace Najm.GraphQL.ApplicationCore._Interfaces;
public interface IAccidentResolver : IAsyncDisposable
{
    IQueryable<AccidentInfo> GetAccident(List<string> caseNumbers);
    IQueryable<AccidentParty> GetAccidentPartiesAsync([Parent] AccidentInfo accidentInfo);
        // IQueryable<AccidentParty> GetAccidentPartiesAsync([Parent] AccidentInfo accidentInfo,
        //  [ScopedService] IAppReadOnlyDbContext dbContext);
}
public interface IAccidentService 
{
    Task<List<AccidentInfo>> GetAccident(AccidentsInput input);
   // Task<AccidentParty> GetAccidentPartiesAsync([Parent] AccidentInfo accidentInfo);

}
