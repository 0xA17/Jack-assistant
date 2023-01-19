using System;
using System.IO;

namespace Jack.ProjectDirectory
{
    class ProjectDir
    {
        #region Переменный

        public static readonly String PDirectory;

        #endregion

        static ProjectDir()
        {
            var appdataPath = Environment.GetEnvironmentVariable("APPDATA");

            if (String.IsNullOrEmpty(appdataPath))
            {
                return;
            }

            var targetDir = $"{appdataPath}\\Dune";

            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);
            }

            PDirectory = targetDir;
        }
    }
}
