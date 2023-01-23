using System;
using System.Windows;
using System.Windows.Input;
using Jack.Core;
using Jack.MVVM.ViewModel;
using Jack.Style.Blure;
using Jack.MVVM.ViewModel.Windows;
using System.Threading;

namespace Jack
{
    public partial class MainWindow : Window
    {
        private static MainWindow Instance;
        public static readonly SynchronizationContext _synchronizationContext;

        public MainWindow()
        {
            Instance = this;
            InitializeComponent();
            new JackCore();
            DataContext = new MainViewModel();
            new WindowsCore();
            InitNavigateUri();
        }

        static MainWindow()
        {
            _synchronizationContext = SynchronizationContext.Current;
        }

        public static MainWindow GetInstance()
        {
            return Instance;
        }

        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            new WindowBlureffect(this, AccentState.ACCENT_ENABLE_BLURBEHIND) { BlurOpacity = 100 };
            MainViewModel.ChangeWindowFrame("HomeNavigation");
        }

        #region Обработка событий

        private void Border_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonMin_Click(Object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void rdMicrophoneOff_Click(Object sender, RoutedEventArgs e)
        {
            MainWindowVievModel.ChangeMicrophoneMode(GetInstance(), true);
        }

        private void rdMicrophoneOn_Click(Object sender, RoutedEventArgs e)
        {
            MainWindowVievModel.ChangeMicrophoneMode(GetInstance(), false);
        }

        #endregion

        private void InitNavigateUri()
        {
            HomeNavigation.Navigate(new Uri("MVVM/View/Pages/HomePage.xaml", UriKind.RelativeOrAbsolute));
            ProgramNavigation.Navigate(new Uri("MVVM/View/Pages/ProgramsPage.xaml", UriKind.RelativeOrAbsolute));
            SettingNavigation.Navigate(new Uri("MVVM/View/Pages/SettingsPage.xaml", UriKind.RelativeOrAbsolute));
            SitesNavigation.Navigate(new Uri("MVVM/View/Pages/SitesPage.xaml", UriKind.RelativeOrAbsolute));
        }
    }
}