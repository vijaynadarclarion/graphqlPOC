using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CorrelationId;
using CorrelationId.Abstractions;
using Microsoft.Extensions.Options;

namespace Adf.Core.ApiClients
{
    /// <summary>
    /// use the X-Correlation-ID HTTP header to trace requests as they move down the stack. 
    /// The CorrelationIdDelegatingHandler is used to take the correlation ID for the current HTTP request and pass it down to the request made in the API to API call.
    /// </summary>
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        private readonly ICorrelationContextAccessor correlationContextAccessor;
        private readonly IOptions<CorrelationIdOptions> options;

        public CorrelationIdDelegatingHandler(
            ICorrelationContextAccessor correlationContextAccessor,
            IOptions<CorrelationIdOptions> options)
        {
            this.correlationContextAccessor = correlationContextAccessor;
            this.options = options;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(this.options.Value.RequestHeader))
            {
                request.Headers.Add(this.options.Value.RequestHeader, this.correlationContextAccessor.CorrelationContext.CorrelationId);
            }

            // Else the header has already been added due to a retry.

            return base.SendAsync(request, cancellationToken);
        }
    }
}
