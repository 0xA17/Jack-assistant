using Jack.MVVM.ViewModel;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Jack.Pages
{
    /// <summary>
    /// Логика взаимодействия для SitesPage.xaml
    /// </summary>
    public partial class SitesPage : Page
    {
        public static SitesPage Instance;

        public SitesPage()
        {
            InitializeComponent();
            DataContext = MainWindow.Instance.DataContext;
            Instance = this;
        }

        public static SitesPage GetInstance()
        {
            return Instance;
        }

        public void Page_Loaded(Object sender, RoutedEventArgs e)
        {
            //DataContext = MainViewModel.GetInstance();
        }

        public void RefreshListView()
        {
            SiteList.ItemsSource = MainViewModel.SiteItem;
        }
    }
}