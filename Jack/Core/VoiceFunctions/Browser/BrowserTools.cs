using Jack.Tools.MemoryOperation;
using Jack.Tools.XML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Jack.Core.VoiceFunctions.Browser
{
    class BrowserTools
    {
        #region Переменные

        private const String GoogleSearchStr = "https://www.google.com/search?q=";

        #endregion

        private static String SearchQueryValidation(String searchQuery, in XElement findSynonymNode)
        {
            if (String.IsNullOrEmpty(searchQuery) ||
                findSynonymNode is null)
            {
                return String.Empty;
            }

            var textIsContains = XMLTools.GetTextIsContains(searchQuery, findSynonymNode);

            if (String.IsNullOrEmpty(textIsContains))
            {
                return String.Empty;
            }

            var resultArr = searchQuery.Split(XMLTools.GetTextIsContains(searchQuery, findSynonymNode));

            if (resultArr.Length > Byte.MinValue)
            {
                return resultArr.Last();
            }

            return String.Empty;
        }

        private static Boolean BrowserSearchQuery(String searchQuery)
        {
            if (String.IsNullOrEmpty(searchQuery))
            {
                return false;
            }

            return ProcessTools.StartProcessInfo(GoogleSearchStr + searchQuery);
        }

        public static Boolean InitBrowserSearch(String text, in XElement findSynonymNode)
        {
            if (String.IsNullOrEmpty(text) ||
                findSynonymNode is null)
            {
                return false;
            }

            return BrowserSearchQuery(SearchQueryValidation(text, findSynonymNode));
        }
    }
}
