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

namespace Jack.MVVM.ViewModel.Pages
{
    class SettingsPageViewModel : SettingsCore
    {
        #region Переменные

        private const String Filter = "Jack command (*.jack)|*.jack";

        public const String ComandStateButtonName = "ComandStateButton";

        #endregion

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

        public static Boolean EditRecognizeState(String buttonName, Boolean isChecked, ToggleButton button = null)
        {
            if (String.IsNullOrEmpty(buttonName))
            {
                return false;
            }

            ChangeLabelState(buttonName, isChecked, removeNameLen: 6, addToName: "Label");
            SpeechEngine.ChangeRecognizeState(isChecked);

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
                    EditRecognizeState(sender.Name, (Boolean)sender.IsChecked);
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
