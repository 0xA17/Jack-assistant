using System;
using System.Windows.Controls;

namespace Jack.Core.ThreadUtils
{
    class Invokes
    {
        #region TextBlock

        public static void EditTextBlock_Text(TextBlock prb, String value)
        {
            if (prb is null || value is null)
            {
                return;
            }

            MainWindow.GetInstance().Dispatcher.Invoke((Action)(() =>
            {
                try
                {
                    prb.Text = value;
                }
                catch {/*SKIP*/}
            }));
        }

        public static void EditLabel_Text(Label prb, String value)
        {
            if (prb is null || value is null)
            {
                return;
            }

            MainWindow.GetInstance().Dispatcher.Invoke((Action)(() =>
            {
                try
                {
                    prb.Content = value;
                }
                catch {/*SKIP*/}
            }));
        }

        #endregion
    }
}
