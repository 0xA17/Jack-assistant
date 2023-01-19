using Jack.Core;
using Jack.MVVM.Model;
using Jack.MVVM.ViewModel;
using Jack.MVVM.ViewModel.Windows;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Jack.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddNewProgramWindow.xaml
    /// </summary>
    public partial class NewProgramWindow : Window
    {

        #region Переменные

        private static NewProgramWindow Instance;

        #endregion

        public NewProgramWindow()
        {
            InitializeComponent();
            MainViewModel.IsEditProgram = false;
            Instance = this;
        }

        public static NewProgramWindow GetInstance()
        {
            return Instance;
        }

        private void CloseButton_Click(Object sender, RoutedEventArgs e)
        {
            ProgramsPageWorkModel.IsProgItemModelAdded = false;
            this.Close();
        }

        private void SaveUserDataButton_Click(Object sender, RoutedEventArgs e)
        {
            NewProgramWindowViewModel.InitSaveUserData(GetInstance(), false, null,
                ref ProgramName_TextBox, ref ProgramPath_TextBox, ref PogramSynonymsTextBox,
                ref ProgramNameBad, ref ProgramPathBad, ref SynonymsBad, ref ProgramPathImg);
        }

        private void ProgramName_GotKeyboardFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            ProgramsPageWorkModel.HideMessageBadData(ProgramNameBad, ProgramName_TextBox);
        }

        private void PogramSynonyms_GotKeyboardFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            ProgramsPageWorkModel.HideMessageBadData(SynonymsBad, PogramSynonymsTextBox);
        }

        private void ShowDialog_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            NewProgramWindowViewModel.InitSelectUserProgram(ref ProgramPathBad, ref ProgramPath_TextBox, ref ProgramPathImg);
        }
    }
}