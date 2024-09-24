using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Specification;
using Najm.GraphQL.ApplicationCore.Entity;

namespace Najm.GraphQL.ApplicationCore.Accidents.Specifications
{
    public class AccidentPartySpec : Specification<AccidentParty>
    {
        public AccidentPartySpec(int caseInfoId) : base()
        {

            Query.Where(q => q.CaseInfoId == caseInfoId);
            Query.Take(1);

        }

    }
}

