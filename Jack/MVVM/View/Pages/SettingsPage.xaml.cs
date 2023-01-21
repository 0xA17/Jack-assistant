using Jack.Core;
using Jack.Core.Dune;
using Jack.Core.Windows;
using Jack.MVVM.Model;
using Jack.MVVM.ViewModel.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Jack.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        #region Переменные

        private static SettingsPage Instance;

        #endregion

        public SettingsPage()
        {
            InitializeComponent();
            Instance = this;
        }

        public static SettingsPage GetInstance()
        {
            return Instance;
        }

        private void ImportDataButton_Click(Object sender, RoutedEventArgs e)
        {
            SettingsPageViewModel.ImportUserData();
        }

        private void ExportDataButton_Click(Object sender, RoutedEventArgs e)
        {
            SettingsPageViewModel.ExportUserData();
        }

        private void EditSynthesizerRateSlider_ValueChanged(Object sender, RoutedPropertyChangedEventArgs<Double> e)
        {
            SpeechEngine.SetSynthesizerRate((Int16)((Slider)sender).Value);
        }

        private void ToggleButton_Checked(Object sender, RoutedEventArgs e)
        {
            SettingsPageViewModel.ButtonStateChange((ToggleButton)sender);
        }
    }
}