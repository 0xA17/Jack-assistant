using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Jack.Tools.XML
{
    static class XmlDocumentExtensions
    {
        public static Boolean TrySave(this XmlDocument xmlDocument, String path)
        {
            if (xmlDocument is null ||
               String.IsNullOrEmpty(path))
            {
                return false;
            }

            try
            {
                xmlDocument.Save(path);

                if (!File.Exists(path))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean TryLoad(this XmlDocument xmlDocument, Stream inStream)
        {
            if (inStream is null ||
                xmlDocument is null)
            {
                return false;
            }

            try
            {
                xmlDocument.Load(inStream);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean TryLoad(this XmlDocument xmlDocument, String path)
        {
            if (String.IsNullOrEmpty(path) ||
                xmlDocument is null)
            {
                return false;
            }

            var tempData = xmlDocument.InnerXml;

            try
            {
                xmlDocument.Load(path);
            }
            catch
            {
                xmlDocument.InnerXml = tempData;
                return false;
            }

            return true;
        }
    }
}
