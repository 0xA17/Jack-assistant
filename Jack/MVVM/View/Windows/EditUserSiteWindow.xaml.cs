using Jack.MVVM.Model;
using Jack.MVVM.ViewModel.Windows;
using Jack.Tools.StringTLS;
using System;
using System.Windows;
using System.Windows.Input;

namespace Jack.MVVM.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditUserSiteWindow.xaml
    /// </summary>
    public partial class EditUserSiteWindow : Window
    {
        #region Переменные

        public static SiteItemModel TargetSiteItemModel = new SiteItemModel();

        #endregion

        public EditUserSiteWindow(SiteItemModel progItemModel)
        {
            InitializeComponent();
            TargetSiteItemModel = progItemModel;
            InitWinData();
        }

        private void InitWinData()
        {
            SiteName_TextBox.Text = TargetSiteItemModel.Name;
            SitePath_TextBox.Text = TargetSiteItemModel.Link;
            UrlPathImg.Source = TargetSiteItemModel.ImageSource;
            SiteSynonymsTextBox.Text = StringTools.GetDataFromStrArr(TargetSiteItemModel.Synonyms);
        }

        private void CloseButton_Click(Object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void SaveUserDataButton_Click(Object sender, RoutedEventArgs e)
        {
            EditUserSiteWindowViewModel.InitSaveUserData(this, true, TargetSiteItemModel,
                ref SiteName_TextBox, ref SitePath_TextBox, ref SiteSynonymsTextBox,
                ref SiteNameBad, ref SitePathBad, ref SynonymsBad);
        }

        private void SiteName_GotKeyboardFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            ProgramsPageWorkModel.HideMessageBadData(SiteNameBad, SiteName_TextBox);
        }

        private void SiteSynonyms_GotKeyboardFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            ProgramsPageWorkModel.HideMessageBadData(SynonymsBad, SiteSynonymsTextBox);
        }

        private void DeleteSite(Object sender, RoutedEventArgs e)
        {
            EditUserSiteWindowViewModel.DeleteUserSite(this, TargetSiteItemModel);
        }
    }
}