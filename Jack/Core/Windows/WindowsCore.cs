using Jack.Core.Windows;
using Jack.Windows;
using System;
using System.Windows;

namespace Jack.Core
{
    public class WindowsCore
    {
        #region Методы

        public static void ShowMessageBox(String title, String context, MessageBoxImage messageBoxImage)
        {
            if (title is null || context is null)
            {
                //throw new ArgumentNullException();
                return;
            }

            MessageBox.Show(context, title, MessageBoxButton.OK, messageBoxImage, MessageBoxResult.Yes);
        }

        public static Boolean ShowOwnerWindows(Window childWindow)
        {
            if (childWindow is null)
            {
                return false;
            }

            var mainWindowInstance = MainWindow.GetInstance();

            if (mainWindowInstance is null)
            {
                return false;
            }

            childWindow.Owner = mainWindowInstance;
            childWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            WindowTools.AddBlurEffectForWindow(mainWindowInstance);
            childWindow.ShowDialog();

            return true;
        }

        #endregion
    }
}