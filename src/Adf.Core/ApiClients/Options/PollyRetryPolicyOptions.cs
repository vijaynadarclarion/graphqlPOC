using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adf.Core.ApiClients.Options
{
    public class PollyRetryPolicyOptions
    {
        public int Count { get; set; } = 3;
        public int BackoffPower { get; set; } = 2;
    }
}
