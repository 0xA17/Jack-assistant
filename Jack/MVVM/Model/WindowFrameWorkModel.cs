using System;
using System.Windows.Controls;

namespace Jack.MVVM.Model
{
    class WindowFrameWorkModel
    {
        public static void ChangeFrameVisible(String targetFrameName, Frame[] frames)
        {
            if (String.IsNullOrEmpty(targetFrameName))
            {
                throw new ArgumentNullException(nameof(targetFrameName));
            }

            if (frames is null)
            {
                throw new ArgumentNullException(nameof(frames));
            }

            foreach (var frame in frames)
            {
                if (frame.Name == targetFrameName)
                {
                    frame.Visibility = System.Windows.Visibility.Visible;
                    continue;
                }

                frame.Visibility = System.Windows.Visibility.Hidden;
            }
        }
    }
}
