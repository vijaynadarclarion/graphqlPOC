using System.Linq;
using System.Text;

namespace Microsoft.AspNetCore.Http
{
    public static class HeaderDictionaryExtensions
    {
        public static string GetRequestHeaders(this IHeaderDictionary dictionary)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var header in dictionary.Where(p => p.Key != "Authorization"))
            {
                sb.AppendLine($"{header.Key}:{header.Value}");
            }
            return sb.ToString();
        }
    }
}
