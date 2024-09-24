using Adf.Core.Data;
using System;
using System.Linq;

namespace Adf.Core.CommonServices.Entities
{
    public class ShortUrl : EntityBase<int>
    {
        public ShortUrl(string url)
        {
            this.Url = url;
            Generate();

        }
        public string Url { get; set; }
        public string ShortenedUrl { get; set; }
        public int Clicks { get; set; } = 0;

        public DateTime? LastAccessedDate { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public void Generate()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            ShortenedUrl = new string(Enumerable.Repeat(chars, 6).Select(s => s[new Random().Next(s.Length)]).ToArray());
        }

    }
}
