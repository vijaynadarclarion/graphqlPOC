using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Najm.GraphQL.ApplicationCore.Entity;

namespace Najm.GraphQL.ApplicationCore.Accidents.Specifications
{
    public class AccidentsSpec : Specification<AccidentInfo>
    {
        public AccidentsSpec(List<string> caseNumbers) : base()
        {

            Query.Where(q => caseNumbers.Contains(q.CaseNumber));

        }

    }
}

