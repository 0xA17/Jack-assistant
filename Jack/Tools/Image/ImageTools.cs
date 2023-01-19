using System;
using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;
using System.Runtime.InteropServices;

namespace Jack.Tools.Image
{
    class ImageTools
    {
        /// <summary>
        /// Конвертирует Bitmap в ImageSource.
        /// </summary>
        /// <param name="bmp">Целевой Bitmap</param>
        /// <returns>Сконвертировванное изоражение в ImageSource</returns>
        public static ImageSource ImageSourceForBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();

            try
            {
                ImageSource newSource = Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(handle);

                return newSource;
            }
            catch
            {
                DeleteObject(handle);
            }

            return null;
        }

        #region DllTool

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern Boolean DeleteObject([In] IntPtr hObject);

        #endregion
    }
}
