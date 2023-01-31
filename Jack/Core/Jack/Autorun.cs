using Jack.ProjectDirectory;
using Jack.Tools.MemoryOperation;
using Jack.Tools.Registry;
using System;

namespace Jack.Core.Jack
{
    class Autorun
    {
        public const String RegistryRunPath = "Software\\Microsoft\\Windows\\CurrentVersion\\Run";

        private static Boolean _isAutoRun;
        public static Boolean IsAutoRun 
        {
            get { return _isAutoRun; }
            set
            {
                _isAutoRun = value;
                ChangeIsAutoRunState(_isAutoRun);
            }
        }

        public static readonly String AppName;

        static Autorun()
        {
            AppName = ProcessTools.GetCurrentName();
        }

        private static void ChangeIsAutoRunState(Boolean state)
        {
            if (state)
            {
                RegistryTools.SetRegistryValue(
                    RegistryRunPath,
                    AppName,
                    ProjectDir.CurrentAppDir);
                return;
            }

            RegistryTools.DeleteRegistryValue(RegistryRunPath, AppName);
        }
    }
}
