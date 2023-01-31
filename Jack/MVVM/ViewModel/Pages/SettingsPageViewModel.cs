using Jack.Core.Dune;
using Jack.Core;
using Jack.Pages;
using Jack.Tools.Dialogs;
using Jack.Tools.XML;
using System;
using System.Windows;
using Jack.Dictionary.СustomCommand;
using System.Windows.Controls.Primitives;
using Jack.Core.ThreadUtils;
using Jack.Core.Jack;

namespace Jack.MVVM.ViewModel.Pages
{
    class SettingsPageViewModel : SettingsCore
    {
        #region Переменные

        private const String Filter = "Jack command (*.jack)|*.jack";

        public const String ComandStateButtonName = "ComandStateButton";

        public const String DataSaveButtonName = "DataSaveButton";

        public const String AutorunButtonName = "AutorunButton";

        #endregion

        public static void InitCheckIsAutoRunState(ToggleButton buttonName)
        {
            if (buttonName is null)
            {
                return;
            }

            if (CheckIsAutoRunState(buttonName))
            {
                buttonName.IsChecked = true;
                ButtonStateChange(buttonName);
            }
        }

        public static void ImportUserData()
        {
            var tmpFileDialog = FileDialogs.ExecuteOpenFileDialog(false, Filter, null);

            if (tmpFileDialog is null)
            {
                return;
            }

            if (!UserCommands.ImportCommands(tmpFileDialog.FileName))
            {
                WindowsCore.ShowMessageBox(
                    "Некорректный файл!",
                    "Ошибка при загрузке команд, файл поврежден!",
                    MessageBoxImage.Warning);
            }
        }

        public static Boolean EditButtonState(String buttonName, Boolean isChecked, ToggleButton button = null)
        {
            if (String.IsNullOrEmpty(buttonName))
            {
                return false;
            }

            ChangeLabelState(buttonName, isChecked, removeNameLen: 6, addToName: "Label");

            switch (buttonName)
            {
                case ComandStateButtonName:
                    SpeechEngine.ChangeRecognizeState(isChecked);
                    break;
                case DataSaveButtonName:
                    UserCommands.IsSaveData = isChecked;
                    break;
                case AutorunButtonName:
                    Autorun.IsAutoRun = isChecked;
                    break;
                default:
                    return false;
            }
            
            if (button is null)
            {
                return false;
            }

            return Invokes.UpdateButtonChecked(SettingsPage._synchronizationContext, button, isChecked);
        }

        public static void ButtonStateChange(ToggleButton sender)
        {
            if (sender is null)
            {
                return;
            }

            switch (sender.Name)
            {
                case ComandStateButtonName:
                    EditButtonState(sender.Name, (Boolean)sender.IsChecked);
                    break;
                case DataSaveButtonName:
                    EditButtonState(sender.Name, (Boolean)sender.IsChecked);
                    break;
                case AutorunButtonName:
                    EditButtonState(sender.Name, (Boolean)sender.IsChecked);
                    break;
                default:
                    break;
            }
        }

        public static void ExportUserData()
        {
            var tmpSaveFileDialog = FileDialogs.ExecuteSaveFileDialog(Filter);

            if (tmpSaveFileDialog is null)
            {
                return;
            }

            if (!UserCommands.ExportCommands(tmpSaveFileDialog.FileName))
            {
                WindowsCore.ShowMessageBox(
                    "Ошибка экспорта!",
                    "Ошибка при сохранении данных!",
                    MessageBoxImage.Warning);
            }
        }
    }
}