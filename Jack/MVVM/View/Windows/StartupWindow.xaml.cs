using Jack.MVVM.ViewModel.Windows;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
