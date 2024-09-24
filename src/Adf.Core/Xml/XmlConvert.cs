using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Adf.Core.Xml
{
    public static class XmlConvert<T>
    {
        public static string Serialize(T o)
        {
            var sw = new StringWriter();
            XmlTextWriter tw = null;
            try
            {
                var serializer = new XmlSerializer(o.GetType());
                tw = new XmlTextWriter(sw);
                serializer.Serialize(tw, o);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sw.Close();
                tw?.Close();
            }

            return sw.ToString();
        }

        public static T Deserialize(string xml)
        {
            StringReader strReader = null;
            XmlTextReader xmlReader = null;
            T obj = default(T);
            try
            {
                strReader = new StringReader(xml);
                var serializer = new XmlSerializer(typeof(T));
                xmlReader = new XmlTextReader(strReader);
                obj = serializer.Deserialize(xmlReader).To<T>();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                xmlReader?.Close();
                strReader?.Close();
            }

            return obj;
        }
    }
}
