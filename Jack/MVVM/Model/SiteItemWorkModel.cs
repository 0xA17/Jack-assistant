using Jack.Tools.StringTLS;
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Jack.MVVM.Model
{
    static class SiteItemWorkModel
    {
        public static Boolean CreateModel(
            this SiteItemModel siteItemModel, 
            String name, 
            Guid siteId, 
            ImageSource imageSource,
            String synonyms,
            String link)
        {
            if (siteItemModel is null ||
                String.IsNullOrEmpty(synonyms) ||
                imageSource is null ||
                synonyms.Trim(' ').Length == 0 ||
                String.IsNullOrEmpty(name) ||
                String.IsNullOrEmpty(link))
            {
                return false;
            }

            siteItemModel.Name = name.Trim(' ').Trim(' ');
            siteItemModel.SiteId = siteId;
            siteItemModel.ImageSource = imageSource;
            siteItemModel.Synonyms = synonyms.Trim(' ').Split(' ');
            siteItemModel.Link = link;

            return true;
        }

        public static SiteItemModel CreateNewModel(
            String name,
            Guid siteId,
            ImageSource imageSource,
            String synonyms,
            String link)
        {
            if (String.IsNullOrEmpty(synonyms) ||
                synonyms.Trim(' ').Length == 0 ||
                String.IsNullOrEmpty(name) ||
                String.IsNullOrEmpty(link))
            {
                return null;
            }

            return new SiteItemModel
            {
                Name = name.Trim(' ').Trim(' '),
                SiteId = siteId,
                ImageSource = imageSource,
                Synonyms = synonyms.Trim(' ').Split(' '),
                Link = link
            };
        }

        public static Boolean CompareSiteItem(SiteItemModel newSiteItemModel, SiteItemModel targetSiteItemModel)
        {
            if (newSiteItemModel is null || targetSiteItemModel is null)
            {
                return false;
            }

            if (newSiteItemModel.Name == targetSiteItemModel.Name &&
                newSiteItemModel.SiteId == targetSiteItemModel.SiteId &&
                StringTools.CompareArray(newSiteItemModel.Synonyms, targetSiteItemModel.Synonyms) &&
                newSiteItemModel.Link == targetSiteItemModel.Link)
            {
                return false;
            }

            return true;
        }

        public static Boolean CheckIsExistSite(ObservableCollection<SiteItemModel> siteItem, String targetPath)
        {
            if (String.IsNullOrEmpty(targetPath))
            {
                return false;
            }

            if (siteItem.Count == 0)
            {
                return false;
            }

            foreach (var item in siteItem)
            {
                if (item.Link.Contains(targetPath))
                {
                    return true;
                }
            }

            return false;
        }
    }
}