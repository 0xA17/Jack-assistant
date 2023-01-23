using System;
using System.Windows;

namespace Jack.MVVM.Model
{
    class StartupWindowWorkModel
    {
        public enum TargetAction : Byte
        {
            Close,
            Show
        }

        protected static Boolean ChangeTargetWindow(Window oldWindow, Window newWindow)
        {
            if (oldWindow is null ||
                newWindow is null)
            {
                return false;
            }

            EditWindow(oldWindow, TargetAction.Close);
            EditWindow(newWindow, TargetAction.Show);

            return true;
        }

        #region Invoke

        public static void EditWindow(Window prb, TargetAction targetAction)
        {
            MainWindow.GetInstance().Dispatcher.Invoke((Action)(() =>
            {
                try
                {
                    if (targetAction == TargetAction.Close)
                    {
                        prb.Close();
                    }
                    else if (targetAction == TargetAction.Show)
                    {
                        prb.Show();
                    }
                }
                catch {/*SKIP*/}
            }));
        }

        #endregion
    }
}
