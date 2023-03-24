using Jack.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using Jack.Tools.Image;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Jack.Tools.XML
{
    static class XMLTools
    {
        /// <summary>
        /// Возвращает коллекцию программ пользователя.
        /// </summary>
        /// <param name="xmlDocument">XML-документ, содержащий структуру программ</param>
        /// <returns>Коллекция программ пользователя</returns>
        public static ObservableCollection<ProgItemModel> GetUserProgram(in XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException();
            }

            var tmpProgItemModel = new ObservableCollection<ProgItemModel>();

            foreach (XmlNode node in xmlDocument.DocumentElement.GetElementsByTagName("Program"))
            {
                try
                {
                    var tmpXElement = XElement.Parse(node.InnerXml.Insert(node.InnerXml.Length, "</Program>").Insert(0, "<Program>"));

                    tmpProgItemModel.Add(new ProgItemModel
                    {
                        Name = tmpXElement.Element("name").Value,
                        ProgramId = Guid.Parse(tmpXElement.Element("ProgramId").Value),
                        ImageSource = ImageTools.ImageSourceForBitmap(Icon.ExtractAssociatedIcon(tmpXElement.Element("link").Value).ToBitmap()),
                        Synonyms = GetSynonyms(tmpXElement.Elements("synonym")),
                        Link = tmpXElement.Element("link").Value,
                        ProcessName = tmpXElement.Element("ProcessName").Value,
                    });
                }
                catch
                {
                    /*SKIP*/
                }
            }

            return tmpProgItemModel;
        }

        public static ObservableCollection<SiteItemModel> GetUserSites(in XmlDocument xmlDocument)
        {
            if (xmlDocument == null)
            {
                throw new ArgumentNullException();
            }

            var tmpSiteItemModel = new ObservableCollection<SiteItemModel>();

            foreach (XmlNode node in xmlDocument.DocumentElement.GetElementsByTagName("Site"))
            {
                try
                {
                    var tmpXElement = XElement.Parse(node.InnerXml.Insert(node.InnerXml.Length, "</Site>").Insert(0, "<Site>"));

                    tmpSiteItemModel.Add(new SiteItemModel
                    {
                        Name = tmpXElement.Element("name").Value,
                        SiteId = Guid.Parse(tmpXElement.Element("SiteId").Value),
                        ImageSource = Imaging.CreateBitmapSourceFromHIcon(Properties.Resources.BrowserPage.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions()) ?? null,
                        Synonyms = GetSynonyms(tmpXElement.Elements("synonym")),
                        Link = tmpXElement.Element("link").Value
                    });
                }
                catch
                {
                    /*SKIP*/
                }
            }

            return tmpSiteItemModel;
        }

        /// <summary>
        /// Возвращает массив слов для распознавания.
        /// </summary>
        /// <param name="xelement">Массив данных))</param>
        /// <returns>Массив слов для распознавания</returns>
        private static String[] GetSynonyms(IEnumerable<XElement> xelement)
        {
            var tmpStrArr = new String[xelement.Count()];

            for (Int32 i = 0; i < tmpStrArr.Length; i++)
            {
                tmpStrArr[i] = xelement.ElementAt(i).Value;
            }

            return tmpStrArr;
        }

        /// <summary>
        /// Добавляет новую команду в XML-документ.
        /// </summary>
        /// <param name="xmlDocument">XML-документ с исходной структурой</param>
        /// <param name="structureXML">Структура XML</param>
        /// <param name="elementId">GUID элемента</param>
        /// <param name="name">Имя программы</param>
        /// <param name="synonymArr">Массив слов для распознавания</param>
        /// <param name="link">Путь к программе</param>
        /// <param name="processName">Имя процесса</param>
        /// <returns>XML-документ, содержащий команды</returns>
        public static XmlDocument AddNewCommand(
            in XmlDocument xmlDocument,
            in String[] structureXML,
            in Guid elementId,
            in String name,
            in String[] synonymArr,
            in String link,
            in String processName)
        {
            if (xmlDocument == null || String.IsNullOrEmpty(name) || structureXML == null || structureXML.Length == 0 ||
                synonymArr == null || synonymArr.Length == 0 || String.IsNullOrEmpty(link) /*|| String.IsNullOrEmpty(processName)*/)
            {
                throw new ArgumentNullException();
            }

            return AddCommand(in xmlDocument, in structureXML, in elementId, in name, in synonymArr, in link, in processName);
        }

        /// <summary>
        /// Добавляет новую команду в XML-документ.
        /// </summary>
        /// <param name="xmlDocument">XML-документ с исходной структурой</param>
        /// <param name="structureXML">Структура XML</param>
        /// <param name="elementId">GUID прогрпммы</param>
        /// <param name="name">Имя программы</param>
        /// <param name="synonymArr">Массив слов для распознавания</param>
        /// <param name="link">Путь к программе</param>
        /// <param name="processName">Имя процесса</param>
        /// <returns>XML-документ, содержащий команды</returns>
        private static XmlDocument AddCommand(
            in XmlDocument xmlDocument,
            in String[] structureXML,
            in Guid elementId,
            in String name,
            in String[] synonymArr,
            in String link,
            in String processName)
        {
            XmlDocument tmpXmlDoc = xmlDocument;
            XmlNode userPrograms = xmlDocument.DocumentElement.SelectSingleNode(structureXML[0]);
            XmlElement itemXml = tmpXmlDoc.CreateElement(structureXML[1]);
            XmlElement programIdXml = tmpXmlDoc.CreateElement(structureXML[6]);
            XmlElement nameXml = tmpXmlDoc.CreateElement(structureXML[2]);
            XmlElement linkXml = tmpXmlDoc.CreateElement(structureXML[4]);
            XmlElement processNameXml = CreateElements(ref tmpXmlDoc, ref structureXML[5]);

            programIdXml.InnerText = elementId.ToString();
            nameXml.InnerText = name;
            linkXml.InnerText = link;

            if (processNameXml != null)
            {
                processNameXml.InnerText = processName;
            }

            itemXml.AppendChild(programIdXml);
            itemXml.AppendChild(nameXml);
            AppendChildElementFromArray(structureXML[3], synonymArr, ref itemXml, ref tmpXmlDoc);
            itemXml.AppendChild(linkXml);

            if (processNameXml != null)
            {
                itemXml.AppendChild(processNameXml);
            }

            userPrograms.AppendChild(itemXml);
            tmpXmlDoc.DocumentElement.AppendChild(userPrograms);

            return tmpXmlDoc;
        }

        private static XmlElement CreateElements(ref XmlDocument xmlDocument, ref String str)
        {
            if (xmlDocument is null || str == String.Empty)
            {
                throw new ArgumentNullException();
            }

            if (str is not null)
            {
                return xmlDocument.CreateElement(str);
            }

            return null;
        }

        /// <summary>
        /// Добавляет элемент в конец XML-документа.
        /// </summary>
        /// <param name="elementName">Имя элемента</param>
        /// <param name="inputArray">Массив слов для распознавания</param>
        /// <param name="xmlElement">Добавляемый XML-элемент</param>
        /// <param name="xmlDocument">Целевой XML-документ</param>
        private static void AppendChildElementFromArray(String elementName, String[] inputArray, ref XmlElement xmlElement, ref XmlDocument xmlDocument)
        {
            XmlNode synonym = null;

            foreach (var strItem in inputArray)
            {
                synonym = xmlDocument.CreateElement(elementName);
                synonym.InnerText = strItem;
                xmlElement.AppendChild(synonym);
            }
        }

        /// <summary>
        /// Удаляет кастомную команду.
        /// </summary>
        /// <param name="xmlDocument">Целевой XML-документ</param>
        /// <param name="commandId">GIUD команды</param>
        /// <param name="targetNode">Целевой XML-узел</param>
        public static void DeleteCommand(ref XmlDocument xmlDocument, String commandId, String targetNode)
        {
            if (xmlDocument == null || String.IsNullOrEmpty(commandId) || String.IsNullOrEmpty(targetNode))
            {
                throw new ArgumentNullException();
            }

            foreach (XmlNode node in xmlDocument.DocumentElement.SelectSingleNode(targetNode))
            {
                if (node.FirstChild.FirstChild.InnerText == commandId)
                {
                    xmlDocument.DocumentElement.SelectSingleNode(targetNode).RemoveChild(node);
                }
            }
        }

        /// <summary>
        /// Проверяет, содержится ли заданная строка в представленном элементе Xml.
        /// </summary>
        /// <param name="comparableText">Целевая строка</param>
        /// <param name="xelement">Представляемый элемент Xml</param>
        /// <returns>Статус выполнения</returns>
        public static Boolean SynonymIsContains(String comparableText, XElement xelement)
        {
            if (String.IsNullOrEmpty(comparableText) || xelement is null)
            {
                return false;
            }

            foreach (var item in xelement.Elements("synonym"))
            {
                if (comparableText.Contains(item.Value))
                {
                    return true;
                }
            }

            return false;
        }

        public static String GetTextIsContains(String comparableText, XElement xelement)
        {
            if (String.IsNullOrEmpty(comparableText) || xelement is null)
            {
                return String.Empty;
            }

            var lastElement = String.Empty;

            foreach (var item in xelement.Elements("synonym"))
            {
                if (comparableText.Contains(item.Value))
                {
                    lastElement = item.Value;
                }
            }

            return lastElement;
        }

        /// <summary>
        /// Проверяет, содержится ли заданная программа в представленном массива элемента Xml.
        /// </summary>
        /// <param name="comparableText">Целевая программа</param>
        /// <param name="xelement">Представляемый элемент Xml</param>
        /// <returns>Статус выполнения</returns>
        public static XElement ChechIsExistProgram(String comparableText, IEnumerable<XElement> xelement)
        {
            if (String.IsNullOrEmpty(comparableText) || xelement is null)
            {
                return null;
            }

            foreach (var item in xelement.Elements("synonym"))
            {
                if (comparableText.Contains(item.Value))
                {
                    return item;
                }
            }

            return null;
        }

        public static Boolean TryParse(String text, out XElement element)
        {
            element = null;

            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            try
            {
                element = XElement.Parse(text);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}