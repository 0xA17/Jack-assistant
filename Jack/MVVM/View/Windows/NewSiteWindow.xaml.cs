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
    /// Логика взаимодействия для NewSiteWindow.xaml
    /// </summary>
    public partial class NewSiteWindow : Window
    {
        public static NewSiteWindow Instance;

        public NewSiteWindow()
        {
            Instance = this;
            InitializeComponent();
            MainViewModel.IsEditSite = false;
        }

        public static NewSiteWindow GetInstance()
        {
            return Instance;
        }

        private void CloseButton_Click(Object sender, RoutedEventArgs e)
        {
            SiteWorkModel.IsSiteItemModelAdded = false;
            Close();
        }

        private void SaveUserDataButton_Click(Object sender, RoutedEventArgs e)
        {
            NewSiteViewModel.InitSaveUserData(GetInstance(), false, null,
                ref SiteName_TextBox, ref SitePath_TextBox, ref SiteSynonymsTextBox,
                ref SiteNameBad, ref SitePathBad, ref SynonymsBad);
        }

        private void SiteName_GotKeyboardFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            SiteNameBad.Visibility = Visibility.Hidden;
            SiteName_TextBox.BorderBrush = Brushes.Transparent;
        }

        private void SiteSynonyms_GotKeyboardFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            SynonymsBad.Visibility = Visibility.Hidden;
            SiteSynonymsTextBox.BorderBrush = Brushes.Transparent;
        }

        private void SitePath_TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SitePathBad.Visibility = Visibility.Hidden;
            SitePath_TextBox.BorderBrush = Brushes.Transparent;
        }
    }
}