using System;
using Jack.Tools.MemoryOperation;
using Microsoft.Win32;

namespace Jack.Tools.Registry
{
    class RegistryTools
    {
        public static readonly String CurrentAppDir;

        static RegistryTools()
        {
            CurrentAppDir = ProcessTools.GetCurrentAppDir().Replace("dll", "exe");
        }

        public static Boolean SetRegistryValue(String subKey, String valueName, String valuePath)
        {
            if (String.IsNullOrEmpty(subKey)    || 
                String.IsNullOrEmpty(valueName) ||
                String.IsNullOrEmpty(valuePath))
            {
                return false;
            }

            try
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(subKey, true);
                key.SetValue(valueName, valuePath);
                key.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean DeleteRegistryValue(String subKey, String valueName)
        {
            if (String.IsNullOrEmpty(subKey) ||
                String.IsNullOrEmpty(valueName))
            {
                return false;
            }

            try
            {
                var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(subKey, true);

                if (key.GetValue(valueName) is not null)
                {
                    key.DeleteValue(valueName);
                }

                key.Close();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static String GetRegistryValue(String subKey, String valueName)
        {
            if (String.IsNullOrEmpty(subKey)  || 
                String.IsNullOrEmpty(valueName))
            {
                return String.Empty;
            }

            var path = String.Empty;

            try
            {
                var readKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(subKey);
                path = (String)readKey.GetValue(valueName);
                readKey.Close();
            }
            catch
            {
                return String.Empty;
            }

            return path;
        }
    }
}
