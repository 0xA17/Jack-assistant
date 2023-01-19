using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace Jack.Core.Windows
{
    class WindowTools
    {
        public static void ShutdownThisApp() => System.Windows.Application.Current.Shutdown();

        /// <summary>
        /// Применяет эффект размытия в окну.
        /// </summary>
        /// <param name="win">Целевое окно</param>
        public static void AddBlurEffectForWindow(Window win)
        {
            if (win == null)
            {
                throw new ArgumentNullException();
            }

            var blurEffect = new BlurEffect();

            blurEffect.Radius = 8;
            win.Effect = blurEffect;
        }
    }
}