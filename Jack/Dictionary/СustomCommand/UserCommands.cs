using Jack.Core;
using Jack.Core.Dune;
using Jack.MVVM.ViewModel;
using Jack.Pages;
using Jack.ProjectDirectory;
using Jack.Tools.XML;
using System;
using System.IO;
using System.Reflection;
using System.Windows;

namespace Jack.Dictionary.СustomCommand
{
    class UserCommands
    {
        #region Переменные

        private static readonly String UserCommandsFilePath;

        public static Boolean IsSaveData = true;

        private const String FileName = "UserCommands.xml";

        private const String UserCommandAssemblyName = "Jack.Dictionary.СustomCommand.СustomCommandDictionary.xml";

        #endregion

        static UserCommands()
        {
            UserCommandsFilePath = $"{ProjectDir.PDirectory}\\{FileName}";

            if (!CheckIsCommandsFileExists())
            {
                ExportCommands(UserCommandsFilePath);
            }
        }

        public static Boolean CheckIsCommandsFileExists()
        {
            return File.Exists(UserCommandsFilePath);
        }

        public static Boolean SaveCommands()
        {
            return ExportCommands(UserCommandsFilePath);
        }

        public static Boolean LoadUserCommand()
        {
            if (!CheckIsCommandsFileExists())
            {
                if (!Commands.СustomCommandDictionaryXML.TryLoad(Assembly.GetExecutingAssembly()?.GetManifestResourceStream(UserCommandAssemblyName) ?? null))
                {
                    return false;
                }

                return ExportCommands(UserCommandsFilePath);
            }

            return ImportCommands(UserCommandsFilePath);
        }

        public static Boolean ImportCommands(String path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return false;
            }

            if (!Commands.СustomCommandDictionaryXML.TryLoad(path) ||
                !XMLTools.TryParse(Commands.СustomCommandDictionaryXML.InnerXml, out Commands.СustomCommandDictionary) ||
                !MainViewModel.InitUserProgram() || !MainViewModel.InitUserSite() /*||
                !Commands.LoadGrammer()*/)
            {
                return false;
            }

            var programsPage = ProgramsPage.GetInstance();
            var sitePage = SitesPage.GetInstance();

            if (programsPage is not null) 
            {
                programsPage.RefreshListView();
            }

            if (sitePage is not null)
            {
                sitePage.RefreshListView();
            }

            return SaveCommands();
        }

        public static Boolean ExportCommands(String path)
        {
            if (String.IsNullOrEmpty(path))
            {
                return false;
            }

            if (!IsSaveData)
            {
                return true;
            }

            return Commands.СustomCommandDictionaryXML.TrySave(path);
        }
    }
}
