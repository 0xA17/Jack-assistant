using Jack.Tools.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Jack.Tools.MemoryOperation
{
    class ProcessTools
    {
        public static void TurnOffPC(Byte delay)
        {
            Process.Start("shutdown", $"/s /t {delay}");
        }

        /// <summary>
        /// Запускает целевой процесс.
        /// </summary>
        /// <param name="path">Путь</param>
        /// <returns>Статус выполнения</returns>
        public static Boolean StartProcess(String path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return false;
            }

            try
            {
                Process.Start(path);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean StartProcessInfo(String url, Boolean createNoWindow = true)
        {
            if (String.IsNullOrEmpty(url))
            {
                return false;
            }

            try
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return false;
                }

                Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace(" ", "%20")}") { CreateNoWindow = createNoWindow });
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Убивает целевой процесс.
        /// </summary>
        /// <param name="name">Имя процесса</param>
        /// <returns>Статус выполнения</returns>
        public static Boolean KillProcess(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return false;
            }

            var tmpProc = Process.GetProcessesByName(name).FirstOrDefault();

            if (tmpProc == null)
            {
                return false;
            }

            try
            {
                tmpProc.Kill();
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Закрывает окна процесса.
        /// </summary>
        /// <param name="name">Имя процесса</param>
        /// <returns>Статус выполнения</returns>
        public static Boolean CloseProgramWindow(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return false;
            }

            var tmpProc = Process.GetProcessesByName(name);

            if (tmpProc.Length == 0)
            {
                return false;
            }

            foreach (var item in tmpProc)
            {
                try
                {
                    item.CloseMainWindow();
                    item.Close();
                }
                catch
                {
                    return false;
                }
            }

            return true;
        }
    }
}
