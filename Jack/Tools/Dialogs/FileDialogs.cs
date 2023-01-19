using System;
using Microsoft.Win32;

namespace Jack.Tools.Dialogs
{
    class FileDialogs
    {
        public static OpenFileDialog ExecuteOpenFileDialog(Boolean multiselect, String filter, String initialDirectory)
        {
            var openFileDialog = new OpenFileDialog();

            openFileDialog.Multiselect = multiselect;
            openFileDialog.Filter = filter ?? String.Empty;
            openFileDialog.InitialDirectory = initialDirectory ?? String.Empty;

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog;
            }

            return null;
        }

        public static SaveFileDialog ExecuteSaveFileDialog(String filter)
        {
            var saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = filter ?? String.Empty;

            if (saveFileDialog.ShowDialog() == true)
            {
                return saveFileDialog;
            }

            return null;
        }
    }
}