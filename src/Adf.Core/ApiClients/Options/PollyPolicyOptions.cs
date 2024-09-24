using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adf.Core.ApiClients.Options
{
    public class PollyPolicyOptions
    {
        public CircuitBreakerPolicyOptions HttpCircuitBreaker { get; set; }

        public PollyRetryPolicyOptions HttpRetry { get; set; }
    }
}
