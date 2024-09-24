﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adf.Core.ApiClients.Options
{
    public class CircuitBreakerPolicyOptions
    {
        public TimeSpan DurationOfBreak { get; set; } = TimeSpan.FromSeconds(30);

        public int ExceptionsAllowedBeforeBreaking { get; set; } = 12;
    }
}
