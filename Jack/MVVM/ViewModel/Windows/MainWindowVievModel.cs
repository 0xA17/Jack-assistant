using Jack.Core;
using System;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using Jack.Core.Dune;
using System.Speech.Recognition;

namespace Jack.MVVM.ViewModel.Windows
{
    class MainWindowVievModel
    {
        public static void ChangeMicrophoneMode(MainWindow mainWindow, Boolean isEnable)
        {
            if (mainWindow is null)
            {
                return;
            }

            var borderColor = String.Empty;
            var doneCmdLabel = String.Empty;

            if (isEnable)
            {
                borderColor = "#1a2b2a";
                doneCmdLabel = "- Джек слушает команды";
            }
            else
            {
                borderColor = "#2b1a2a";
                doneCmdLabel = "- Джек отдыхаает";
            }

            EditRecognizeMode(isEnable,
                mainWindow.MicrophoneBorder,
                new SolidColorBrush((Color)ColorConverter.ConvertFromString(borderColor)),
                mainWindow.doneCmdLabel, doneCmdLabel,
                mainWindow.rdMicrophoneOff, mainWindow.rdMicrophoneOn);
        }

        private static Boolean EditRecognizeMode(
            Boolean isEnable,
            Border border, SolidColorBrush solidColor, 
            Label dialogLabel, String duneContent,
            RadioButton rdMicrophoneOff, 
            RadioButton rdMicrophoneOn)
        {
            if (solidColor is null ||
                dialogLabel is null ||
                border is null ||
                rdMicrophoneOff is null ||
                rdMicrophoneOn is null ||
                String.IsNullOrEmpty(duneContent))
            {
                return false;
            }

            border.Background = solidColor;
            dialogLabel.Content = duneContent;

            if (isEnable)
            {
                if (!SpeechEngine.StartRecognize())
                {
                    return false;
                }

                SpeechEngine.ChangeRecognizeState(true);
                rdMicrophoneOff.Visibility = Visibility.Hidden;
                rdMicrophoneOn.Visibility = Visibility.Visible;

                return true;
            }

            SpeechEngine.StopRecognize();
            SpeechEngine.ChangeRecognizeState(false);
            rdMicrophoneOff.Visibility = Visibility.Visible;
            rdMicrophoneOn.Visibility = Visibility.Hidden;

            return true;
        }
    }
}