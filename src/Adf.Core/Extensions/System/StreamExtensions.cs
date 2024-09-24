using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adf.Core.Extensions.System
{
    public static class StreamExtensions
    {
        public static string GetFileExtension(this Stream stream)
        {
            // Read 16 bytes into an array   
            byte[] data = new byte[0x10];
            stream.Position = 0;
            int read = stream.Read(data, 0, data.Length);

            string data_as_hex = BitConverter.ToString(data);
            string magicCheck = data_as_hex.Substring(0, 11);

            //Set the contenttype based on File Extension
            switch (magicCheck)
            {
                case "FF-D8-FF-E1":
                    return "jpg";
                case "FF-D8-FF-E0":
                    return "jpg";
                case "2F-39-6A-2F":
                    return "jpg";
                case "25-50-44-46":
                    return "pdf";
                case "89-50-4E-47":
                    return "png";
                case "D0-CF-11-E0-A1-B1-1A-E1":
                    return "doc";
                case "50-4B-03-04":
                    return "docx";
                default:
                    break;
            }

            // Read more bytes for further checking, SVG files can have a longer header
            byte[] largerBuffer = new byte[512];
            stream.Position = 0;
            read = stream.Read(largerBuffer, 0, largerBuffer.Length);
            string headerString = Encoding.UTF8.GetString(largerBuffer);

            if (headerString.Contains("<svg") || headerString.Contains("<?xml"))
            {
                return "svg";
            }

            return string.Empty;
        }
    }
}
