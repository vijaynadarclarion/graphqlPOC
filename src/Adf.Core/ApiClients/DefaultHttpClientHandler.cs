using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Adf.Core.ApiClients
{
    public class DefaultHttpClientHandler : HttpClientHandler
    {
        public DefaultHttpClientHandler() => this.AutomaticDecompression =
          //  DecompressionMethods.Brotli |
            DecompressionMethods.Deflate |
            DecompressionMethods.GZip;
    }
}
