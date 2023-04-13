using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Jack.Core.ThreadUtils
{
    class Invokes
    {
        #region TextBlock

        public static Boolean UpdateWindowVisibility(SynchronizationContext context, Window window, Visibility visibility)
        {
            if (context is null ||
                window is null)
            {
                return false;
            }

            try
            {
                context.Post(state =>
                {
                    window.Visibility = visibility;
                }, null);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean UpdateButtonChecked(SynchronizationContext context, ToggleButton toggleButton, Boolean isChecked)
        {
            if (context is null ||
                toggleButton is null)
            {
                return false;
            }

            try
            {
                context.Post(state =>
                {
                    toggleButton.IsChecked = isChecked;
                }, null);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean UpdateLabelContext(SynchronizationContext context, Label label, String text)
        {
            if (context is null ||
                label is null ||
                String.IsNullOrEmpty(text))
            {
                return false;
            }

            try
            {
                context.Post(state =>
                {
                    label.Content = text;
                }, null);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Label GetLabelByName(Page page, String name)
        {
            if (page is null ||
                String.IsNullOrEmpty(name))
            {
                return null;
            }

            Label targetLabelButton = null;

            page.Dispatcher.Invoke((Action)(() =>
            {
                try
                {
                    targetLabelButton = (Label)page.FindName(name);
                }
                catch {/*SKIP*/}
            }));

            return targetLabelButton;
        }

        public static void UpdateTextBlockText(TextBlock prb, String value)
        {
            if (prb is null || value is null)
            {
                return;
            }

            MainWindow.Instance.Dispatcher.Invoke((Action)(() =>
            {
                try
                {
                    prb.Text = value;
                }
                catch {/*SKIP*/}
            }));
        }

        public static void UpdateLabelContext(System.Windows.Controls.Label prb, String value)
        {
            if (prb is null || value is null)
            {
                return;
            }

            MainWindow.Instance.Dispatcher.Invoke((Action)(() =>
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
