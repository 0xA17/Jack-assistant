using Jack.Core;
using Jack.Core.Dune;
using Jack.MVVM.Model;
using Jack.MVVM.View.Windows;
using Jack.Windows;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Jack.MVVM.ViewModel.Windows
{
    class EditUserSiteWindowViewModel : SiteWorkModel
    {
        public static void InitSaveUserData(
            EditUserSiteWindow editUserSite, Boolean IsEdit, SiteItemModel targetSiteItemModel,
            ref TextBox siteNameTextBox, ref TextBox sitePathTextBox, ref TextBox siteSynonymsTextBox,
            ref Label siteNameBad, ref Label sitePathBad, ref Label synonymsBad)
        {
            if (SaveUserData(
                IsEdit, targetSiteItemModel, ref siteNameTextBox,
                ref sitePathTextBox, ref siteSynonymsTextBox, ref siteNameBad,
                ref sitePathBad, ref synonymsBad))
            {
                editUserSite.Close();
            }
        }

        public static void DeleteUserSite(EditUserSiteWindow editUserSite, SiteItemModel targetSiteItemModel)
        {
            if (editUserSite is null ||
                targetSiteItemModel is null)
            {
                return;
            }

            editUserSite.Visibility = Visibility.Hidden;

            if (!Commands.DeleteСustomCommand(targetSiteItemModel.SiteId.ToString(), "UserSites"))
            {
                return;
            }

            MainViewModel.SiteItem.Remove(targetSiteItemModel);
            editUserSite.Close();
        }

        public static void EditSite(SiteItemModel siteItemModel)
        {
            if (siteItemModel is null)
            {
                return;
            }

            WindowsCore.ShowOwnerWindows(new EditUserSiteWindow(siteItemModel));
            MainWindow.GetInstance().Effect = null;
        }
    }
}