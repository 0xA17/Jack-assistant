using System;
using Jack.MVVM.Model;
using System.Windows.Controls;
using Jack.Windows;
using System.Windows;
using System.Windows.Media;
using Jack.Core.Dune;
using Jack.Core;

namespace Jack.MVVM.ViewModel.Windows
{
    class NewProgramWindowViewModel : ProgramsPageWorkModel
    {
        public static void InitSaveUserData(
            NewProgramWindow newProgramWindow, Boolean IsEdit, ProgItemModel targetProgItemModel,
            ref TextBox programNameTextBox, ref TextBox programPathTextBox, ref TextBox pogramSynonymsTextBox,
            ref Label programNameBad, ref Label programPathBad, ref Label synonymsBad, ref Image programPathImg)
        {
            if (SaveUserData(
                IsEdit, targetProgItemModel, ref programNameTextBox,
                ref programPathTextBox, ref pogramSynonymsTextBox, ref programNameBad,
                ref programPathBad, ref synonymsBad, ref programPathImg))
            {
                newProgramWindow.Close();
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

        public static void NewProgram()
        {
            WindowsCore.ShowOwnerWindows(new NewProgramWindow());

            if (IsProgItemModelAdded)
            {
                var tmpProgItemModel = TMPProgItemModel;

                if (!Commands.
                    AddСustomCommand(tmpProgItemModel.ProgramId,
                    tmpProgItemModel.Name, tmpProgItemModel.Synonyms,
                    tmpProgItemModel.Link, tmpProgItemModel.ProcessName, 
                    Commands.TargetCommand.Program))
                {
                    return;
                }

                MainViewModel.ProgItem.Add(tmpProgItemModel);
                TMPProgItemModel = new ProgItemModel();
                IsProgItemModelAdded = false;
            }

            MainWindow.GetInstance().Effect = null;
            MainViewModel.IsEditProgram = !MainViewModel.IsEditProgram;
        }
    }
}