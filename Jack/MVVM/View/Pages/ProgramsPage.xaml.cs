using Jack.MVVM.ViewModel;
using System.Windows.Controls;

namespace Jack.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProgramsPage.xaml
    /// </summary>
    public partial class ProgramsPage : Page
    {
        public static ProgramsPage Instance;

        public ProgramsPage()
        {
            InitializeComponent();
            DataContext = MainWindow.GetInstance().DataContext;
            Instance = this;
        }

        public static ProgramsPage GetInstance()
        {
            return Instance;
        }

        public void RefreshListView()
        {
            ProgramList.ItemsSource = MainViewModel.ProgItem;
        }
    }
}