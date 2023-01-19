using AudioSwitcher.AudioApi.CoreAudio;
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

            Int32 currentVolume = CurrentVolume();
            Int32 addVolumeValue = 0;

            foreach (var item in data.Split(' '))
            {
                if (Int32.TryParse(item, out addVolumeValue))
                {
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
            }

            return false;
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

            Int32 currentVolume = CurrentVolume();
            Int32 removeVolumeValue = 0;

            foreach (var item in data.Split(' '))
            {
                if (Int32.TryParse(item, out removeVolumeValue))
                {
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
            }

            return false;
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

            UInt32 removeVolumeValue = 0;

            foreach (var item in data.Split(' '))
            {
                if (UInt32.TryParse(item, out removeVolumeValue))
                {
                    SetVolume((Int32)removeVolumeValue);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Возвращает актуальное значение громкости системы.
        /// </summary>
        /// <returns>Актуальное значение громкости системы</returns>
        private static Int32 CurrentVolume()
        {
            var device = new CoreAudioController().DefaultPlaybackDevice;

            return (Int32)device.Volume;
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
