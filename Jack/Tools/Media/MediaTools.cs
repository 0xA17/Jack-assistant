using AudioSwitcher.AudioApi.CoreAudio;
using Jack.Core.Settings;
using Jack.Tools.StringTLS;
using PluginInterface;
using System;
using System.Runtime.InteropServices;

namespace Jack.Tools.Media
{
    class MediaTools
    {
        #region Переменные

        public const Int32 KEYEVENTF_EXTENTEDKEY = 1;
        public const Int32 KEYEVENTF_KEYUP = 0;
        public const Int32 VK_MEDIA_NEXT_TRACK = 0xB0;
        public const Int32 VK_MEDIA_PLAY_PAUSE = 0xB3;
        public const Int32 VK_MEDIA_PREV_TRACK = 0xB1;

        #endregion

        #region Keybd_event

        [DllImport("user32.dll")]
        public static extern void keybd_event(Byte virtualKey, Byte scanCode, UInt32 flags, IntPtr extraInfo);

        public static void MEDIA_NEXT_TRACK()
        {
            keybd_event(VK_MEDIA_NEXT_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        public static void MEDIA_PREV_TRACK()
        {
            keybd_event(VK_MEDIA_PREV_TRACK, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        public static void MEDIA_PLAY_PAUSE()
        {
            keybd_event(VK_MEDIA_PLAY_PAUSE, 0, KEYEVENTF_EXTENTEDKEY, IntPtr.Zero);
        }

        #endregion

        /// <summary>
        /// Добавляет к уровню громкости системы.
        /// </summary>
        /// <param name="data">Данные для обработки</param>
        /// <returns>Статус выполнения</returns>
        public static Boolean AddVolume(String data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return false;
            }

            Int16 currentVolume = CurrentVolume();
            Int16 addVolumeValue = StringTools.GetValueFromStr(data);

            if (addVolumeValue == 0)
            {
                return false;
            }

            if (currentVolume + addVolumeValue < 100)
            {
                SetVolume(currentVolume + addVolumeValue);
            }
            else
            {
                SetVolume(100);
            }

            return true;
        }

        /// <summary>
        /// Вычитает из уровню громкости системы.
        /// </summary>
        /// <param name="data">Данные для обработки</param>
        /// <returns>Статус выполнения</returns>
        public static Boolean RemoveVolume(String data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return false;
            }

            Int16 currentVolume = CurrentVolume();
            Int16 removeVolumeValue = StringTools.GetValueFromStr(data);

            if (removeVolumeValue == 0)
            {
                return false;
            }

            if (currentVolume - removeVolumeValue > 0)
            {
                SetVolume(currentVolume - removeVolumeValue);
            }
            else
            {
                SetVolume(0);
            }

            return true;
        }

        /// <summary>
        /// Устанавливает громкость системы.
        /// </summary>
        /// <param name="data">Данные для обработки</param>
        /// <returns>Статус выполнения</returns>
        public static Boolean SetUpVolume(String data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return false;
            }

            Int16 removeVolumeValue = StringTools.GetValueFromStr(data);

            if (removeVolumeValue > 100 ||
                removeVolumeValue < 0)
            {
                return false;
            }

            SetVolume(removeVolumeValue);

            return true;
        }

        /// <summary>
        /// Возвращает актуальное значение громкости системы.
        /// </summary>
        /// <returns>Актуальное значение громкости системы</returns>
        private static Int16 CurrentVolume()
        {
            var device = new CoreAudioController().DefaultPlaybackDevice;

            return (Int16)device.Volume;
        }

        /// <summary>
        /// Задает громкость системы.
        /// </summary>
        /// <param name="volume">Значение громкости</param>
        private static void SetVolume(Int32 volume)
        {
            var device = new CoreAudioController().DefaultPlaybackDevice;

            device.Volume = volume;
        }
    }
}
