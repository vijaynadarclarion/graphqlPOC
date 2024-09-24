using Microsoft.Extensions.Configuration;
using System;

namespace Adf.Core
{ 
    /// <summary>
    /// Adf Options
    /// </summary>
    public class AdfOptions
    {
        public string ApplicationRootUrl { get; set; }
        public string SystemName { get; set; }
        public string ApplicationName { get; set; }
        public string CommonsDbReadOnlyConnectionStringName { get; set; }
        public string CommonsDbConnectionStringName { get; set; }

        public string TenantId { get; set; }
        public string DefaultCulture { get; set; }
        public string Version { get; set; }
        public SwaggerOptions Swagger { get; set; }
    }
}
