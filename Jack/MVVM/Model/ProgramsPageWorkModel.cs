using Jack.Core.Dune;
using Jack.MVVM.ViewModel;
using Jack.Tools.Image;
using Jack.Tools.StringTLS;
using System;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using System.IO;
using Microsoft.Win32;
using Jack.Dictionary;

namespace Jack.MVVM.Model
{
    class ProgramsPageWorkModel
    {
        #region Переменные

        private static OpenFileDialog TMPFileDialog = null;
        public static Boolean IsProgItemModelAdded = false;
        public static ProgItemModel TMPProgItemModel = new ProgItemModel();

        #endregion

        #region [New/Edit]ProgramWindows

        protected static Boolean SaveUserData(
            Boolean IsEdit, ProgItemModel targetProgItemModel,
            ref TextBox programNameTextBox, ref TextBox programPathTextBox, ref TextBox pogramSynonymsTextBox,
            ref Label programNameBad, ref Label programPathBad, ref Label synonymsBad, ref System.Windows.Controls.Image programPathImg)
        {
            if (programNameTextBox  is null ||
                programPathTextBox  is null || pogramSynonymsTextBox is null ||
                programNameBad      is null || programPathBad        is null ||
                synonymsBad         is null || programPathImg        is null)
            {
                return false;
            }

            return SaveUserDataInternal
                (
                    IsEdit,
                    targetProgItemModel,
                    ref programNameTextBox,
                    ref programPathTextBox,
                    ref pogramSynonymsTextBox,
                    ref programNameBad,
                    ref programPathBad,
                    ref synonymsBad,
                    ref programPathImg
                );
        }

        private static Boolean SaveUserDataInternal(
            Boolean IsEdit, ProgItemModel targetProgItemModel,
            ref TextBox programNameTextBox, ref TextBox programPathTextBox, ref TextBox pogramSynonymsTextBox,
            ref Label programNameBad, ref Label programPathBad, ref Label synonymsBad, ref Image programPathImg)
        {
            var isPathGood = true;
            var isNameGood = true;
            var isSynonymsGood = true;
            ImageSource tmpImageSource = null;

            if (!StringTools.StringValidation(programNameTextBox.Text))
            {
                programNameBad.Visibility = Visibility.Visible;
                programNameTextBox.BorderBrush = Brushes.Red;
                isNameGood = false;
            }

            if (File.Exists(programPathTextBox.Text))
            {
                tmpImageSource = ImageTools.ImageSourceForBitmap(System.Drawing.Icon.ExtractAssociatedIcon(programPathTextBox.Text).ToBitmap());
                programPathImg.Source = tmpImageSource;
            }
            else
            {
                programPathBad.Visibility = Visibility.Visible;
                programPathTextBox.BorderBrush = Brushes.Red;
                programPathBad.Content = "Некорректный путь!";
                isPathGood = false;
            }

            if (!StringTools.StringValidation(pogramSynonymsTextBox.Text) ||
                StringTools.CharacterCheck(pogramSynonymsTextBox.Text, AllowedCharacters.AllowedSynonymCharacters))
            {
                synonymsBad.Visibility = Visibility.Visible;
                pogramSynonymsTextBox.BorderBrush = Brushes.Red;
                isSynonymsGood = false;
            }

            if (!isPathGood || !isNameGood || !isSynonymsGood)
            {
                return false;
            }

            if (IsEdit && targetProgItemModel is not null)
            {
                if (!InitEditProgItem(
                    targetProgItemModel, 
                    in programNameTextBox, 
                    ref programPathTextBox, 
                    in pogramSynonymsTextBox, 
                    in tmpImageSource, 
                    ref programPathBad))
                {
                    return false;
                }
            }
            else
            {
                if (!InitAddProgItem(
                    ref programPathBad, 
                    ref programPathTextBox, 
                    in programNameTextBox, 
                    tmpImageSource, 
                    in pogramSynonymsTextBox))
                {
                    return false;
                }
            }

            IsProgItemModelAdded = true;
            TMPFileDialog = null;

            return true;
        }

        private static Boolean InitAddProgItem(
            ref Label programPathBad, 
            ref TextBox programPathTextBox, 
            in TextBox programNameTextBox,
            ImageSource imageSource,
            in TextBox pogramSynonymsTextBox)
        {
            if (TMPFileDialog == null)
            {
                //throw new ArgumentNullException();
                return false;
            }

            if (GoCheckIsExistProgram(TMPFileDialog.FileName, ref programPathBad, ref programPathTextBox))
            {
                return false;
            }

            return TMPProgItemModel.CreateModel
                (programNameTextBox.Text, 
                 Guid.NewGuid(), 
                 imageSource, 
                 pogramSynonymsTextBox.Text, 
                 TMPFileDialog.FileName, 
                 TMPFileDialog.SafeFileName.Replace(".exe", ""));
        }

        private static Boolean InitEditProgItem(
            ProgItemModel targetProgItemModel, 
            in TextBox programNameTextBox,
            ref TextBox programPathTextBox,
            in TextBox pogramSynonymsTextBox,
            in ImageSource tmpImageSource,
            ref Label programPathBad)
        {

            var newTMPProgItemModel = ProgItemWorkModel.CreateNewModel(
                programNameTextBox.Text,
                targetProgItemModel.ProgramId,
                tmpImageSource,
                pogramSynonymsTextBox.Text,
                programPathTextBox.Text,
                Path.GetFileName(programPathTextBox.Text).Replace(".exe", ""));

            return EditProgItem(newTMPProgItemModel, targetProgItemModel, ref programPathBad, ref programPathTextBox);
        }

        private static Boolean EditProgItem(ProgItemModel newProgItemModel, ProgItemModel targetProgItemModel, ref Label programPathBad, ref TextBox programPathTextBox)
        {
            if (ProgItemWorkModel.CompareProgItem(newProgItemModel, targetProgItemModel))
            {
                if (newProgItemModel.Link != targetProgItemModel.Link)
                {
                    if (GoCheckIsExistProgram(programPathTextBox.Text, ref programPathBad, ref programPathTextBox))
                    {
                        return false;
                    }
                }

                if (!Commands.DeleteСustomCommand(targetProgItemModel.ProgramId.ToString(), "UserPrograms") ||
                    !Commands.AddСustomCommand(
                        newProgItemModel.ProgramId,
                        newProgItemModel.Name, newProgItemModel.Synonyms,
                        newProgItemModel.Link, newProgItemModel.ProcessName, 
                        Commands.TargetCommand.Program))
                {
                    return false;
                }

                MainViewModel.ProgItem.Remove(targetProgItemModel);
                MainViewModel.ProgItem.Insert(Byte.MinValue, newProgItemModel);
            }

            return true;
        }

        protected static void SelectUserProgram(ref Label programPathBad, ref TextBox programPathTextBox, ref Image programPathImg)
        {
            if (programPathBad is null ||
                programPathTextBox is null ||
                programPathImg is null)
            {
                return;
            }

            SelectUserProgramInternal(ref programPathBad, ref programPathTextBox, ref programPathImg);
        }

        private static void SelectUserProgramInternal(ref Label programPathBad, ref TextBox programPathTextBox, ref Image programPathImg)
        {
            var tmpFileDialog = Tools.Dialogs.FileDialogs.ExecuteOpenFileDialog(false, null, Environment.GetFolderPath(Environment.SpecialFolder.Programs));

            if (tmpFileDialog == null)
            {
                return;
            }

            if (GoCheckIsExistProgram(tmpFileDialog.FileName, ref programPathBad, ref programPathTextBox))
            {
                return;
            }

            programPathTextBox.Text = tmpFileDialog.FileName;
            programPathImg.Source = ImageTools.ImageSourceForBitmap(
                System.Drawing.Icon.ExtractAssociatedIcon(programPathTextBox.Text).ToBitmap());
            TMPFileDialog = tmpFileDialog;
        }

        private static Boolean GoCheckIsExistProgram(String path, ref Label programPathBad, ref TextBox programPathTextBox)
        {
            if (ProgItemWorkModel.CheckIsExistProgram(MainViewModel.ProgItem, path))
            {
                programPathBad.Visibility = Visibility.Visible;
                programPathTextBox.BorderBrush = Brushes.Red;
                programPathBad.Content = "Такая программа уже есть!";

                return true;
            }

            return false;
        }

        #endregion

        #region Other

        public static void HideMessageBadData(Label label, TextBox textBox)
        {
            if (label is null ||
                textBox is null)
            {
                return;
            }

            label.Visibility = Visibility.Hidden;
            textBox.BorderBrush = Brushes.Transparent;
        }

        #endregion
    }
}