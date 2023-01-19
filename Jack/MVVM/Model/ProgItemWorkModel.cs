using System;
using Jack.Tools.StringTLS;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Jack.MVVM.Model
{
    static class ProgItemWorkModel
    {
        public static Boolean CreateModel(
            this ProgItemModel progItemModel,
            String name,
            Guid programId,
            ImageSource imageSource,
            String synonyms,
            String link,
            String processName)
        {
            if (progItemModel is null ||
                String.IsNullOrEmpty(name) ||
                imageSource is null ||
                String.IsNullOrEmpty(synonyms) ||
                synonyms.Trim(' ').Length == 0 ||
                String.IsNullOrEmpty(link) ||
                String.IsNullOrEmpty(processName))
            {
                return false;
            }

            progItemModel.Name = name.Trim(' ').Trim(' ');
            progItemModel.ProgramId = programId;
            progItemModel.ImageSource = imageSource;
            progItemModel.Synonyms = synonyms.Trim(' ').Split(' ');
            progItemModel.Link = link;
            progItemModel.ProcessName = processName;

            return true;
        }

        public static ProgItemModel CreateNewModel(
            String name,
            Guid programId,
            ImageSource imageSource,
            String synonyms,
            String link,
            String processName)
        {
            if (String.IsNullOrEmpty(name) ||
                imageSource is null ||
                String.IsNullOrEmpty(synonyms) ||
                synonyms.Trim(' ').Length == 0 ||
                String.IsNullOrEmpty(link) ||
                String.IsNullOrEmpty(processName))
            {
                return null;
            }

            return new ProgItemModel
            {
                Name = name.Trim(' ').Trim(' '),
                ProgramId = programId,
                ImageSource = imageSource,
                Synonyms = synonyms.Trim(' ').Split(' '),
                Link = link,
                ProcessName = processName
            };
        }

        /// <summary>
        /// Проверяет на наличие изменений между двумя объектами ProgItemModel.
        /// </summary>
        /// <param name="newProgItemModel">Новый объект</param>
        /// <param name="targetProgItemModel">Целевой объект</param>
        /// <returns>True - если изменения найдены, иначе - False</returns>
        public static Boolean CompareProgItem(ProgItemModel newProgItemModel, ProgItemModel targetProgItemModel)
        {
            if (newProgItemModel == null || targetProgItemModel == null)
            {
                return false;
            }

            if (newProgItemModel.Name == targetProgItemModel.Name &&
                newProgItemModel.ProgramId == targetProgItemModel.ProgramId &&
                StringTools.CompareArray(newProgItemModel.Synonyms, targetProgItemModel.Synonyms) &&
                newProgItemModel.Link == targetProgItemModel.Link &&
                newProgItemModel.ProcessName == targetProgItemModel.ProcessName)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Проверяет уникальность программы.
        /// </summary>
        /// <param name="ProgItem">Список актуальных программ</param>
        /// <param name="targetPath">Целевой путь новой программы</param>
        /// <returns></returns>
        public static Boolean CheckIsExistProgram(ObservableCollection<ProgItemModel> ProgItem, String targetPath)
        {
            if (String.IsNullOrEmpty(targetPath))
            {
                return false;
            }

            if (ProgItem.Count == 0)
            {
                return false;
            }

            foreach (var item in ProgItem)
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