using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adf.Core.ApiClients
{
    public class HttpClientOptions
    {
        public Uri BaseAddress { get; set; }

        public TimeSpan Timeout { get; set; }
    }
}
