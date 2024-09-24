using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

namespace Najm.GraphQL.ApplicationCore.CompQuotes.Validators;


using System;
using System.Net.Http.Headers;
using System.Security.Policy;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Localization;

using Polly;

public class CompQuotesRequestValidator : AbstractValidator<CompQuotesRequestDto>
{
    
    public CompQuotesRequestValidator()
    {

      RuleFor(x => x.QuoteRequestSourceID).NotNull().WithErrorCode("NUP_001").WithMessage("QuoteRequestSourceID cannot be null");
        RuleFor(x => x.GroupID).NotNull().WithErrorCode("NUP_001").WithMessage("GroupId cannot be null");
        RuleFor(x => x.RequestReferenceNo).MinimumLength(10).WithMessage(string.Format("RequestReferenceNo length need to be minimum 10"));

        RuleFor(x => x.CoverageTypeID).NotNull().WithErrorCode("NUP_001").WithMessage("CoverageTypeID cannot be null");
         
    }

  
}


