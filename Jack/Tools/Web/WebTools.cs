using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Jack.Tools.Web
{
    class WebTools
    {
        public static Boolean RequestTryCreate(String url, out HttpWebRequest httpWebRequest)
        {
            if (String.IsNullOrEmpty(url))
            {
                httpWebRequest = null;
                return false;
            }

            try
            {
                httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            catch
            {
                httpWebRequest = null;
                return false;
            }

            return true;
        }
    }
}
