using System;

namespace Jack.Tools.Web
{
    class UrlTools
    {
        public static Boolean CheckURLIsCorrect(String url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return false;
            }

            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }
}