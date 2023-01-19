using System;
using Jack.MVVM.Model;
using System.Windows.Controls;
using Jack.Windows;
using System.Windows;
using System.Windows.Media;
using Jack.Core.Dune;
using Jack.Core.Windows;
using Jack.Core;

namespace Jack.MVVM.ViewModel.Windows
{
    class EditUserProgramWindowViewModel : ProgramsPageWorkModel
    {
        public static void InitSaveUserData(
            EditUserProgramWindow editUserProgramWindow, Boolean IsEdit, ProgItemModel targetProgItemModel,
            ref TextBox programNameTextBox, ref TextBox programPathTextBox, ref TextBox pogramSynonymsTextBox,
            ref Label programNameBad, ref Label programPathBad, ref Label synonymsBad, ref Image programPathImg)
        {
            if (SaveUserData(
                IsEdit, targetProgItemModel, ref programNameTextBox, 
                ref programPathTextBox, ref pogramSynonymsTextBox, ref programNameBad,
                ref programPathBad, ref synonymsBad, ref programPathImg))
            {
                editUserProgramWindow.Close();
            }
        }

        public static void InitSelectUserProgram(ref Label programPathBad, ref TextBox programPathTextBox, ref Image programPathImg)
        {
            if (programPathBad is null ||
                programPathTextBox is null ||
                programPathImg is null)
            {
                return;
            }

            programPathBad.Visibility = Visibility.Hidden;
            programPathTextBox.BorderBrush = Brushes.Transparent;

            SelectUserProgram(ref programPathBad, ref programPathTextBox, ref programPathImg);
        }

        public static void DeleteUserProgram(EditUserProgramWindow editUserProgram, ProgItemModel targetProgItemModel)
        {
            if (editUserProgram is null ||
                targetProgItemModel is null)
            {
                return;
            }

            editUserProgram.Visibility = Visibility.Hidden;

            if (!Commands.DeleteСustomCommand(targetProgItemModel.ProgramId.ToString(), "UserPrograms"))
            {
                return;
            }

            MainViewModel.ProgItem.Remove(targetProgItemModel);
            editUserProgram.Close();
        }

        public static void EditProgramm(ProgItemModel progItemModel)
        {
            if (progItemModel is null)
            {
                return;
            }

            WindowsCore.ShowOwnerWindows(new EditUserProgramWindow(progItemModel));
            MainWindow.GetInstance().Effect = null;
        }
    }
}