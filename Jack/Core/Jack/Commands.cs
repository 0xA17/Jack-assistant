using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Speech.Recognition;
using System.Windows;
using System.Xml.Linq;
using System.Xml;
using System.Reflection;
using Jack.Tools.XML;
using Jack.Tools.Media;
using Jack.Tools.StringTLS;
using Jack.Tools.MemoryOperation;
using Jack.Dictionary.СustomCommand;
using Braintree;
using System.Threading;
using Jack.MVVM.ViewModel.Pages;
using Jack.Pages;
using Jack.Core.ThreadUtils;
using Jack.Core.VoiceFunctions.Currency;
using Jack.Core.VoiceFunctions.Browser;

namespace Jack.Core.Dune
{
    class Commands
    {
        #region Переменные

        public enum TargetCommand : UInt16
        {
            Program,
            Site
        }

        private static MainWindow MWInstance 
        {
            get
            {
                return MainWindow.GetInstance();
            }
        }
        public static XElement CommandDictionary;
        public static XElement СustomCommandDictionary;
        public static XmlDocument СustomCommandDictionaryXML = new XmlDocument();
        public static readonly String[] ProgramCommandStructureXML = { "UserPrograms", "Program", "name", "synonym", "link", "ProcessName", "ProgramId" };
        public static readonly String[] SiteCommandStructureXML = { "UserSites", "Site", "name", "synonym", "link", null, "SiteId" };

        #endregion

        public static void RecEngineSpeechRecognize(String result)
        {
            #if DEBUG
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
            #endif

            if (String.IsNullOrEmpty(result))
            {
                return;
            }

            try
            {
                CommandProcessing(result);
            }
            catch
            {
                SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.CommandErrorAnswer));
            }

            #if DEBUG
                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.Elapsed} - [{result}]");
            #endif
        }

        private static void CommandProcessing(String result)
        {
            if (String.IsNullOrEmpty(result))
            {
                return;
            }

            result = result.ToLower();

            if (!XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("StartRecordCommand").First()))
            {
                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("OffPCСommands").First()))
            {
                SpeechEngine.GiveSpeackText($"{StringTools.GiveRandText(AnswerDictionary.OffPCAnswer)}", MWInstance.DuneAnswer);
                //ProcessTools.TurnOffPC();
            }

            {
                var browserFindNode = CommandDictionary.Elements("Browser").Elements("Find").First();

                if (XMLTools.TextIsContains(result,
                    browserFindNode))
                {
                    if (BrowserTools.InitBrowserSearch(result, browserFindNode))
                    {
                        SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.IsFound), MWInstance.DuneAnswer);
                    }

                    return;
                }
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("MediaСommands").First()))
            {
                if (XMLTools.TextIsContains(result,
                    CommandDictionary.Elements("BasicSystemCommands").Elements("NextСommands").First()))
                {
                    MediaTools.MEDIA_NEXT_TRACK();
                    return;
                }

                if (XMLTools.TextIsContains(result,
                    CommandDictionary.Elements("BasicSystemCommands").Elements("PreviousСommands").First()))
                {
                    MediaTools.MEDIA_PREV_TRACK();
                    return;
                }

                if (XMLTools.TextIsContains(result,
                    CommandDictionary.Elements("BasicSystemCommands").Elements("ResumeСommands").First()))
                {
                    MediaTools.MEDIA_PLAY_PAUSE();
                    return;
                }

                if (XMLTools.TextIsContains(result,
                    CommandDictionary.Elements("BasicSystemCommands").Elements("StopMediaСommands").First()))
                {
                    MediaTools.MEDIA_PLAY_PAUSE();
                }

                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("VolumeСommands").First()))
            {
                if (XMLTools.TextIsContains(result,
                    CommandDictionary.Elements("BasicSystemCommands").Elements("AddСommands").First()))
                {
                    MediaTools.AddVolume(result);
                    SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.VolumeAnswer), MWInstance.DuneAnswer);
                    return;
                }

                if (XMLTools.TextIsContains(result,
                    CommandDictionary.Elements("BasicSystemCommands").Elements("RemoveСommands").First()))
                {
                    MediaTools.RemoveVolume(result);
                    SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.VolumeAnswer), MWInstance.DuneAnswer);
                    return;
                }

                if (XMLTools.TextIsContains(result,
                    CommandDictionary.Elements("BasicSystemCommands").Elements("SetСommands").First()))
                {
                    if (!MediaTools.SetUpVolume(result))
                    {
                        SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.VolumeIsBadAnswer), MWInstance.DuneAnswer);
                        return;
                    }

                    SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.VolumeAnswer), MWInstance.DuneAnswer);
                    return;
                }
                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("StartCommand").First()))
            {
                var сustomProgram = XMLTools.ChechIsExistProgram(result, СustomCommandDictionary.Elements("UserPrograms").Elements("Program"));

                if (сustomProgram != null)
                {
                    if (!ProcessTools.StartProcess(сustomProgram.Parent.Elements("link").First().Value))
                    {
                        SpeechEngine.GiveSpeackText($"{StringTools.GiveRandText(AnswerDictionary.IsNotLaunchAnswer)} {сustomProgram.Value}", MWInstance.DuneAnswer);
                        return;
                    }

                    SpeechEngine.GiveSpeackText($"{сustomProgram.Value} {StringTools.GiveRandText(AnswerDictionary.LaunchAnswer)}", MWInstance.DuneAnswer);
                    return;
                }

                var сustomSite = XMLTools.ChechIsExistProgram(result, СustomCommandDictionary.Elements("UserSites").Elements("Site"));

                if (сustomSite != null)
                {
                    if (!ProcessTools.StartProcessInfo(сustomSite.Parent.Elements("link").First().Value))
                    {
                        SpeechEngine.GiveSpeackText($"{StringTools.GiveRandText(AnswerDictionary.IsNotLaunchAnswer)} {сustomSite.Value}", MWInstance.DuneAnswer);
                        return;
                    }

                    SpeechEngine.GiveSpeackText($"{сustomSite.Value} {StringTools.GiveRandText(AnswerDictionary.LaunchAnswer)}", MWInstance.DuneAnswer);
                    return;
                }

                if (XMLTools.TextIsContains(result,
                    CommandDictionary.Elements("BasicSystemCommands").Elements("CalcCommand").First()))
                {
                    ProcessTools.StartProcess("calc.exe");
                    SpeechEngine.GiveSpeackText($"Калькулятор {StringTools.GiveRandText(AnswerDictionary.LaunchAnswer)}", MWInstance.DuneAnswer);
                    return;
                }
            }
            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("CloseCommand").First()))
            {
                var сustomProgram = XMLTools.ChechIsExistProgram(result, СustomCommandDictionary.Elements("UserPrograms").Elements("Program"));

                if (сustomProgram != null)
                {
                    if (ProcessTools.CloseProgramWindow(сustomProgram.Parent.Elements("ProcessName").First().Value))
                    {
                        SpeechEngine.GiveSpeackText($"{сustomProgram.Value} {StringTools.GiveRandText(AnswerDictionary.CloseAnswer)}", MWInstance.DuneAnswer);
                        return;
                    }

                    SpeechEngine.GiveSpeackText($"{сustomProgram.Value} {StringTools.GiveRandText(AnswerDictionary.IsClosedAnswer)}", MWInstance.DuneAnswer);
                    return;
                }

                if (XMLTools.TextIsContains(result,
                    CommandDictionary.Elements("BasicSystemCommands").Elements("CalcCommand").First()))
                {
                    if (!ProcessTools.KillProcess("CalculatorApp"))
                    {
                        SpeechEngine.GiveSpeackText($"Калькулятор {StringTools.GiveRandText(AnswerDictionary.IsClosedAnswer)}", MWInstance.DuneAnswer);
                        return;
                    }

                    SpeechEngine.GiveSpeackText($"Калькулятор {StringTools.GiveRandText(AnswerDictionary.CloseAnswer)}", MWInstance.DuneAnswer);
                    return;
                }
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("DeVisibleCommand").First()))
            {
                if (Invokes.UpdateWindowVisibility(MainWindow._synchronizationContext, MainWindow.GetInstance(), Visibility.Collapsed))
                {
                    SpeechEngine.GiveSpeackText("Программа свернута", MWInstance.DuneAnswer);
                }

                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("VisibleCommand").First()))
            {
                if (Invokes.UpdateWindowVisibility(MainWindow._synchronizationContext, MainWindow.GetInstance(), Visibility.Visible))
                {
                    SpeechEngine.GiveSpeackText("Программа развернута", MWInstance.DuneAnswer);
                }
                
                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("OffVoiceСommands").First()))
            {
                if (SettingsPageViewModel.EditButtonState(SettingsPageViewModel.ComandStateButtonName, false, SettingsPage.GetInstance().ComandStateButton))
                {
                    SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.SilentAnswer), MWInstance.DuneAnswer);
                }

                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("Сurrency").Elements("Course").First()))
            {
                SpeechEngine.GiveSpeackText(RateCurrency.GetRateCurrencyAnswer(result), MWInstance.DuneAnswer);

                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("OnVoiceСommands").First()))
            {
                if (SettingsPageViewModel.EditButtonState(SettingsPageViewModel.ComandStateButtonName, true, SettingsPage.GetInstance().ComandStateButton))
                {
                    SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.OkeyAnswer), MWInstance.DuneAnswer);
                }

                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("IsNotSaveData").First()))
            {
                if (SettingsPageViewModel.EditButtonState(SettingsPageViewModel.DataSaveButtonName, false, SettingsPage.GetInstance().DataSaveButton))
                {
                    SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.OkeyAnswer), MWInstance.DuneAnswer);
                }

                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("IsSaveData").First()))
            {
                if (SettingsPageViewModel.EditButtonState(SettingsPageViewModel.DataSaveButtonName, true, SettingsPage.GetInstance().DataSaveButton))
                {
                    SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.OkeyAnswer), MWInstance.DuneAnswer);
                }

                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("GoodbyeСommands").First()))
            {
                SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.BayAnswer), MWInstance.DuneAnswer);
                Thread.Sleep(2000);
                Process.GetCurrentProcess().Kill();
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("StartRecordCommand").First()) && result.Split(" ").Length == 1)
            {
                SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.HelloAnswer), MainWindow.GetInstance().DuneAnswer);
                return;
            }

            SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.CommandNotFound), MainWindow.GetInstance().DuneAnswer);
        }

        public static Boolean AddСustomCommand(
            Guid cmdId,
            in String name,
            in String[] synonymArr,
            in String link,
            in String processName,
            TargetCommand targetCommand)
        {
            try
            {
                switch (targetCommand)
                {
                    case TargetCommand.Program:
                        СustomCommandDictionaryXML = XMLTools.AddNewCommand
                            (
                                in СustomCommandDictionaryXML,
                                in ProgramCommandStructureXML,
                                in cmdId, in name, in synonymArr, in link, in processName
                            );
                        break;
                    case TargetCommand.Site:
                        СustomCommandDictionaryXML = XMLTools.AddNewCommand
                            (
                                in СustomCommandDictionaryXML,
                                in SiteCommandStructureXML,
                                in cmdId, in name, in synonymArr, in link, null
                            );
                        break;
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }

            if (!XMLTools.TryParse(СustomCommandDictionaryXML.InnerXml, out СustomCommandDictionary))
            {
                return false;
            }

            return UserCommands.SaveCommands();
        }

        public static Boolean DeleteСustomCommand(String itemId, String targetNode)
        {
            try
            {
                XMLTools.DeleteCommand(ref СustomCommandDictionaryXML, itemId, targetNode);
            }
            catch 
            { 
                return false; 
            }

            if (!XMLTools.TryParse(СustomCommandDictionaryXML.InnerXml, out СustomCommandDictionary))
            {
                return false;
            }

            return UserCommands.SaveCommands();
        }

        public static Boolean LoadCommands()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var commandDictionary = new XmlDocument();

            if (!commandDictionary.TryLoad(assembly?.GetManifestResourceStream("Jack.Dictionary.CommandDictionary.xml") ?? null))
            {
                return false;
            }
            if (!UserCommands.LoadUserCommand())
            {
                return false;
            }
            if (!XMLTools.TryParse(commandDictionary.InnerXml, out CommandDictionary))
            {
                return false;
            }
            if (!XMLTools.TryParse(СustomCommandDictionaryXML.InnerXml, out СustomCommandDictionary))
            {
                return false;
            }

            return true;
        }
    }
}