using Jack.Core;
using System;
using Jack.MVVM.Model;
using System.Collections.ObjectModel;
using Jack.Core.Dune;
using GalaSoft.MvvmLight.CommandWpf;
using System.Windows.Controls;
using Jack.Core.Windows;
using Jack.Tools.XML;
using Jack.MVVM.ViewModel.Windows;

namespace Jack.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {
        #region Переменные

        public RelayCommand<RadioButtonSelected> RadioButtonSelect { get; set; }

        public Core.Command.RelayCommand CloseWindow { get; set; }

        private static Frame[] MainWindowFrames
        {
            get
            {
                var mainWindowInstance = MainWindow.Instance;

                return new Frame[]
                {
                    mainWindowInstance.HomeNavigation,
                    mainWindowInstance.ProgramNavigation,
                    mainWindowInstance.SettingNavigation,
                    mainWindowInstance.SitesNavigation
                };
            }
        }

        #region ProgItemModel

        public static Boolean IsEditProgram = true;

        public Core.Command.RelayCommand AddNewProgram { get; set; }

        public static ObservableCollection<ProgItemModel> ProgItem { get; set; }

        private ProgItemModel _progItemModel;

        public ProgItemModel SelectedProgItem
        { 
            get 
            { 
                return _progItemModel; 
            }
            set 
            { 
                _progItemModel = value;
                OnPropertyChanged();
                if (IsEditProgram)
                {
                    EditUserProgramWindowViewModel.EditProgramm(_progItemModel);
                    _progItemModel = null;
                }
            }
        }

        #endregion

        #region SiteItemModel

        public static Boolean IsEditSite = true;

        public Core.Command.RelayCommand AddNewSite { get; set; }

        public static ObservableCollection<SiteItemModel> SiteItem { get; set; }

        private SiteItemModel _siteItemModel;

        public SiteItemModel SelectedSiteItem
        {
            get
            {
                return _siteItemModel;
            }
            set
            {
                _siteItemModel = value;
                OnPropertyChanged();
                if (IsEditSite)
                {
                    EditUserSiteWindowViewModel.EditSite(_siteItemModel);
                    _siteItemModel = null;
                }
            }
        }

        #endregion

        #endregion

        public MainViewModel()
        {
            ProgItem = new ObservableCollection<ProgItemModel>();
            SiteItem = new ObservableCollection<SiteItemModel>();

            InitUserProgram();
            InitUserSite();

            AddNewProgram = new Core.Command.RelayCommand(o =>
            {
                NewProgramWindowViewModel.NewProgram();
            });

            AddNewSite = new Core.Command.RelayCommand(o =>
            {
                NewSiteViewModel.NewSite();
            });

            RadioButtonSelect = new RelayCommand<RadioButtonSelected>(o =>
            {
                ChangeWindowFrame(o.TargetFrameName);
            });

            CloseWindow = new Core.Command.RelayCommand(o =>
            {
                InitCloseWindow();
            });
        }

        public static Boolean InitUserProgram()
        {
            try
            {
                ProgItem = XMLTools.GetUserProgram(in Commands.СustomCommandDictionaryXML);
            }
            catch (ArgumentNullException)
            {
                return false;
            }

            return true;
        }

        public static Boolean InitUserSite()
        {
            try
            {
                SiteItem = XMLTools.GetUserSites(in Commands.СustomCommandDictionaryXML);
            }
            catch (ArgumentNullException)
            {
                return false;
            }

            return true;
        }

        #region InternalMethods

        public static void ChangeWindowFrame(String frameName)
        {
            if (String.IsNullOrEmpty(frameName))
            {
                return;
            }

            WindowFrameWorkModel.ChangeFrameVisible(frameName, MainWindowFrames);
        }

        private void InitCloseWindow()
        {
            WindowTools.ShutdownThisApp();
        }

        #endregion
    }
}