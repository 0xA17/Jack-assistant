using Jack.Core;
using Jack.Core.Dune;
using Jack.MVVM.Model;
using Jack.Windows;
using System;
using System.Windows.Controls;

namespace Jack.MVVM.ViewModel.Windows
{
    class NewSiteViewModel : SiteWorkModel
    {
        public static void InitSaveUserData(
            NewSiteWindow newSiteWindow, Boolean IsEdit, SiteItemModel targetSiteItemModel,
            ref TextBox siteNameTextBox, ref TextBox sitePathTextBox, ref TextBox siteSynonymsTextBox,
            ref Label siteNameBad, ref Label sitePathBad, ref Label synonymsBad)
        {
            if (SaveUserData(
                IsEdit, targetSiteItemModel, ref siteNameTextBox,
                ref sitePathTextBox, ref siteSynonymsTextBox, ref siteNameBad,
                ref sitePathBad, ref synonymsBad))
            {
                newSiteWindow.Close();
            }
        }

        public static void NewSite()
        {
            WindowsCore.ShowOwnerWindows(new NewSiteWindow());

            if (IsSiteItemModelAdded)
            {
                var tmpSiteItemModel = SiteItemTMPModel;

                if (!Commands.
                    AddСustomCommand(tmpSiteItemModel.SiteId,
                    tmpSiteItemModel.Name, tmpSiteItemModel.Synonyms,
                    tmpSiteItemModel.Link, null, Commands.TargetCommand.Site))
                {
                    return;
                }

                MainViewModel.SiteItem.Add(tmpSiteItemModel);
                SiteItemTMPModel = new SiteItemModel();
                IsSiteItemModelAdded = false;
            }

            MainWindow.Instance.Effect = null;
            MainViewModel.IsEditSite = !MainViewModel.IsEditSite;
        }
    }
}