using System;
using System.Windows;
using System.Windows.Input;
using Jack.MVVM.ViewModel.Windows;

namespace Jack.MVVM.View.Windows
{
    /// <summary>
    /// Логика взаимодействия для StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public static StartupWindow Instance { get; private set; }

        public StartupWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        private void Window_Loaded(Object sender, RoutedEventArgs e)
        {
            StartupWindowViewModel.StartProgram();
        }

        private void Window_MouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}