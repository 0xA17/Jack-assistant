using Jack.Tools.StringTLS;
using Jack.MVVM.Model;
using System;
using System.Windows;
using System.Windows.Input;
using Jack.MVVM.ViewModel.Windows;

namespace Jack.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditUserProgramWindow.xaml
    /// </summary>
    public partial class EditUserProgramWindow : Window
    {
        #region Переменные

        private static EditUserProgramWindow Instance;

        public static ProgItemModel TargetProgItemModel = new ProgItemModel();

        #endregion

        public EditUserProgramWindow(ProgItemModel progItemModel)
        {
            if (progItemModel is null)
            {
                Close();
            }

            InitializeComponent();
            Instance = this;
            TargetProgItemModel = progItemModel;
            InitWinData();
        }

        public static EditUserProgramWindow GetInstance()
        {
            return Instance;
        }

        private void InitWinData()
        {
            ProgramName_TextBox.Text = TargetProgItemModel.Name;
            ProgramPath_TextBox.Text = TargetProgItemModel.Link;
            ProgramPathImg.Source = TargetProgItemModel.ImageSource;
            PogramSynonymsTextBox.Text = StringTools.GetDataFromStrArr(TargetProgItemModel.Synonyms);
        }

        private void CloseButton_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveUserDataButton_Click(Object sender, RoutedEventArgs e)
        {
            EditUserProgramWindowViewModel.InitSaveUserData(GetInstance(), true, TargetProgItemModel,
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
            EditUserProgramWindowViewModel.InitSelectUserProgram(ref ProgramPathBad, ref ProgramPath_TextBox, ref ProgramPathImg);
        }

        private void DeleteProgram(Object sender, RoutedEventArgs e)
        {
            EditUserProgramWindowViewModel.DeleteUserProgram(GetInstance(), TargetProgItemModel);
        }
    }
}