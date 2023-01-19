using System;
using Jack.Core.Dune;
using Jack.MVVM.ViewModel;
using Jack.Tools.StringTLS;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using Jack.Dictionary;
using Jack.Tools.Web;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace Jack.MVVM.Model
{
    class SiteWorkModel
    {
        #region Переменные

        public static Boolean IsSiteItemModelAdded = false;
        public static SiteItemModel SiteItemTMPModel = new SiteItemModel();

        #endregion

        #region Методы

        protected static Boolean SaveUserData(
            Boolean IsEdit, SiteItemModel targetProgItemModel,
            ref TextBox siteNameTextBox, ref TextBox sitePathTextBox, ref TextBox siteSynonymsTextBox,
            ref Label siteNameBad, ref Label sitePathBad, ref Label synonymsBad)
        {
            if (siteNameTextBox is null     || sitePathTextBox is null || 
                siteSynonymsTextBox is null || siteNameBad is null     || 
                sitePathBad is null         || synonymsBad is null )
            {
                return false;
            }

            return SaveUserDataInternal
                (
                    IsEdit,
                    targetProgItemModel,
                    siteNameTextBox,
                    sitePathTextBox,
                    siteSynonymsTextBox,
                    siteNameBad,
                    sitePathBad,
                    synonymsBad
                );
        }

        private static Boolean SaveUserDataInternal(
            Boolean IsEdit, SiteItemModel targetSiteItemModel,
            TextBox siteNameTextBox, TextBox sitePathTextBox, TextBox siteSynonymsTextBox,
            Label siteNameBad, Label sitePathBad, Label synonymsBad)
        {
            var isPathGood = true;
            var isNameGood = true;
            var isSynonymsGood = true;

            ImageSource tmpImageSource = null;
            try
            {
                tmpImageSource = Imaging.CreateBitmapSourceFromHIcon(Properties.Resources.BrowserPage.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            } catch {/*SKIP*/}

            if (!StringTools.StringValidation(siteNameTextBox.Text))
            {
                siteNameBad.Visibility = Visibility.Visible;
                siteNameTextBox.BorderBrush = Brushes.Red;
                isNameGood = false;
            }

            if (!UrlTools.CheckURLIsCorrect(sitePathTextBox.Text))
            {
                sitePathBad.Visibility = Visibility.Visible;
                sitePathTextBox.BorderBrush = Brushes.Red;
                sitePathBad.Content = "Некорректная ссылка!";
                isPathGood = false;
            }

            if (!StringTools.StringValidation(siteSynonymsTextBox.Text) ||
                StringTools.CharacterCheck(siteSynonymsTextBox.Text, AllowedCharacters.AllowedSynonymCharacters))
            {
                synonymsBad.Visibility = Visibility.Visible;
                siteSynonymsTextBox.BorderBrush = Brushes.Red;
                isSynonymsGood = false;
            }

            if (!isPathGood || !isNameGood || !isSynonymsGood)
            {
                return false;
            }

            if (IsEdit && targetSiteItemModel is not null)
            {
                if (!InitEditSiteItem(
                    targetSiteItemModel,
                    siteNameTextBox,
                    sitePathTextBox,
                    siteSynonymsTextBox,
                    tmpImageSource,
                    sitePathBad))
                {
                    return false;
                }
            }
            else
            {
                if (!InitAddSiteItem(
                    sitePathBad,
                    sitePathTextBox,
                    siteNameTextBox,
                    tmpImageSource,
                    siteSynonymsTextBox))
                {
                    return false;
                }
            }

            IsSiteItemModelAdded = true;
            return true;
        }

        private static Boolean InitAddSiteItem(
            Label sitePathBad,
            TextBox sitePathTextBox,
            TextBox siteNameTextBox,
            ImageSource imageSource,
            TextBox siteSynonymsTextBox)
        {
            if (!GoCheckIsExistSite(sitePathBad, sitePathTextBox))
            {
                return false;
            }

            return SiteItemTMPModel.CreateModel
                (siteNameTextBox.Text, 
                Guid.NewGuid(), imageSource, 
                siteSynonymsTextBox.Text, 
                sitePathTextBox.Text);
        }

        private static Boolean GoCheckIsExistSite(Label sitePathBad, TextBox sitePathTextBox)
        {
            if (SiteItemWorkModel.CheckIsExistSite(MainViewModel.SiteItem, sitePathTextBox.Text))
            {
                sitePathBad.Visibility = Visibility.Visible;
                sitePathTextBox.BorderBrush = Brushes.Red;
                sitePathBad.Content = "Такой сайт уже есть!";

                return false;
            }

            return true;
        }

        private static Boolean InitEditSiteItem(
            SiteItemModel targetSiteItemModel,
            TextBox siteNameTextBox,
            TextBox sitePathTextBox,
            TextBox siteSynonymsTextBox,
            ImageSource tmpImageSource,
            Label sitePathBad)
        {

            var newTMPProgItemModel = SiteItemWorkModel.CreateNewModel
                (siteNameTextBox.Text, 
                 targetSiteItemModel.SiteId,
                 tmpImageSource,
                 siteSynonymsTextBox.Text,
                 sitePathTextBox.Text);

            return EditSiteItem(newTMPProgItemModel, targetSiteItemModel, sitePathBad, sitePathTextBox);
        }

        private static Boolean EditSiteItem(SiteItemModel newSiteItemModel, SiteItemModel targetSiteItemModel, Label sitePathBad, TextBox sitePathTextBox)
        {
            if (SiteItemWorkModel.CompareSiteItem(newSiteItemModel, targetSiteItemModel))
            {
                if (newSiteItemModel.Link != targetSiteItemModel.Link)
                {
                    if (!GoCheckIsExistSite(sitePathBad, sitePathTextBox))
                    {
                        return false;
                    }
                }

                if (!Commands.DeleteСustomCommand(targetSiteItemModel.SiteId.ToString(), "UserSites") ||
                    !Commands.AddСustomCommand(
                        newSiteItemModel.SiteId,
                        newSiteItemModel.Name, newSiteItemModel.Synonyms,
                        newSiteItemModel.Link, null, 
                        Commands.TargetCommand.Site))
                {
                    return false;
                }

                MainViewModel.SiteItem.Remove(targetSiteItemModel);
                MainViewModel.SiteItem.Insert(Byte.MinValue, newSiteItemModel);
            }

            return true;
        }

        #endregion
    }
}