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

        public static void RecEngineSpeechRecognize(Object sender, SpeechRecognizedEventArgs e)
        {
            #if DEBUG
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
            #endif

            if (sender is null || e is null)
            {
                return;
            }

            try
            {
                CommandProcessing(e.Result.Text);
            }
            catch
            {
                SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.CommandErrorAnswer));
            }

            #if DEBUG
                stopwatch.Stop();
                Console.WriteLine($"{stopwatch.Elapsed} - [{e.Result.Text}]");
            #endif
        }

        public static void CommandProcessing(String result)
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
                    MediaTools.SetUpVolume(result);
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
                    ProcessTools.StartProcess(сustomProgram.Parent.Elements("link").First().Value);
                    SpeechEngine.GiveSpeackText($"{сustomProgram.Value} {StringTools.GiveRandText(AnswerDictionary.LaunchAnswer)}", MWInstance.DuneAnswer);
                    return;
                }

                var сustomSite = XMLTools.ChechIsExistProgram(result, СustomCommandDictionary.Elements("UserSites").Elements("Site"));

                if (сustomSite != null)
                {
                    ProcessTools.StartProcess(сustomSite.Parent.Elements("link").First().Value);
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
                MainWindow.GetInstance().Visibility = Visibility.Collapsed;
                SpeechEngine.GiveSpeackText("Программа свернута", MWInstance.DuneAnswer);
                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("VisibleCommand").First()))
            {
                MainWindow.GetInstance().Visibility = Visibility.Visible;
                SpeechEngine.GiveSpeackText("Программа развернута", MWInstance.DuneAnswer);
                return;
            }
            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("OffVoiceСommands").First()))
            {
                SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.SilentAnswer), MWInstance.DuneAnswer);
                SpeechEngine.RecognizeState = false;
                return;
            }

            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("OnVoiceСommands").First()))
            {
                SpeechEngine.RecognizeState = true;
                SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.OkeyAnswer), MWInstance.DuneAnswer);
                return;
            }
            if (XMLTools.TextIsContains(result,
                CommandDictionary.Elements("BasicSystemCommands").Elements("GoodbyeСommands").First()))
            {
                SpeechEngine.GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.BayAnswer), MWInstance.DuneAnswer);
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