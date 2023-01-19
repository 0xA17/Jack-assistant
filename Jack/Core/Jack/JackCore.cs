using System.Speech.Recognition;
using System;
using System.Globalization;
using Jack.Core.Dune;
using System.Windows;

namespace Jack.Core
{
    class JackCore
    {
        #region Переменные

        private static JackCore Instance;
           
        #endregion

        public JackCore()
        {
            Instance = this;
            TrySetSpeechEngine();
            InitProg();
        }

        private static void TrySetSpeechEngine()
        {
            try
            {
            }
            catch
            {
                WindowsCore.ShowMessageBox(
                    "Отсутствуют библиотеки!",
                    "Ошибка при загрузке необходимой библиотеки для работы программы!\nУбедитесь, что установлены все необходимые зависимые пакеты.",
                    MessageBoxImage.Warning);
                //Application.Current.Shutdown();
            }
        }

        public static JackCore GetInstance()
        {
            return Instance;
        }

        public void InitProg()
        {
            if (!Commands.LoadCommands())
            {
                /*FAIL*/
            }

            Dune.SpeechEngine.InitSpeaker();
        }
    }
}