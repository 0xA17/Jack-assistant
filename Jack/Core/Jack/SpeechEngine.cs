using System;
using System.Threading;
using Jack.Core.ThreadUtils;
using System.Windows.Controls;
using System.Speech.Synthesis;
using System.Speech.Recognition;
using Jack.Tools.StringTLS;
using NAudio.Wave;
using Vosk;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Jack.Core.Settings;

namespace Jack.Core.Dune
{
    class SpeechEngine
    {
        #region Переменные

        private static VoskRecognizer _voiceRecognition;
        private static WaveOut _waveOut;
        private static VoiceAssistantSettings AssistantSettings;
        private static AudioOutSingleton _audioOut;
        private static WaveInEvent _waveIn;
        private static readonly Object SyncRoot = new Object();
        private static Int16 SynthesizerRate = 0;
        private static Boolean RecognizeState = true;

        #endregion

        static SpeechEngine()
        {
            AssistantSettings = new VoiceAssistantSettings();
            SetDefoltInputDevice();
            SetDefoltOutDevice();

            _waveIn = InitAudioInput();
            _voiceRecognition = InitSpeechToText();

            if (_voiceRecognition is null)
            {
                return;
            }

            _audioOut = InitTextToSpeech(_waveOut);

            AddSpeechRecogniz();
            SetSynthesizerRate(Byte.MinValue);
            GiveSpeackText(StringTools.GiveRandText(AnswerDictionary.HelloAnswer), MainWindow.GetInstance().DuneAnswer);
            StartRecognize();
        }

        #region Методы

        public static Boolean InitSpeaker() => true;

        private static void ProcessAudioInput(Object s, WaveInEventArgs waveEventArgs)
        {
            lock (SyncRoot)
            {
                VoskResult newWords;

                if (s is VoskResult simulatedInput)
                {
                    newWords = simulatedInput;
                }
                else
                {
                    var recognitionResult = _voiceRecognition.AcceptWaveform(waveEventArgs.Buffer, waveEventArgs.BytesRecorded);

                    if (!recognitionResult)
                    {
                        return;
                    }

                    var jsonResult = _voiceRecognition.Result();
                    newWords = JsonConvert.DeserializeObject<VoskResult>(jsonResult);
                }

                if (newWords == null ||
                    String.IsNullOrEmpty(newWords.text))
                {
                    return;
                }

                Commands.RecEngineSpeechRecognize(newWords.text);
            }
        }

        private static VoskRecognizer InitSpeechToText()
        {
            Vosk.Vosk.SetLogLevel(AssistantSettings.VoskLogLevel);
            VoskRecognizer rec;

            if (!Directory.Exists(AssistantSettings.ModelFolder))
            {
                Console.WriteLine($"Voice recognition model folder missing: {AssistantSettings.ModelFolder}");

                return null;
            }

            try
            {
                var model = new Model(AssistantSettings.ModelFolder);
                rec = new VoskRecognizer(model, AssistantSettings.AudioInSampleRate);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Can't load model (could it be missing?): {ex.Message}");

                return null;
            }

            rec.SetMaxAlternatives(0);
            rec.SetWords(true);

            return rec;
        }

        private static AudioOutSingleton InitTextToSpeech(WaveOut waveOut)
        {
            var synthesizer = new SpeechSynthesizer();
            var voiceSelected = false;

            Console.WriteLine("\r\nAvailable voices:");

            foreach (var voice in synthesizer.GetInstalledVoices())
            {
                var info = voice.VoiceInfo;
                Console.WriteLine($"- Id: {info.Id} | Name: {info.Name} | Age: {info.Age} | Gender: {info.Gender} | Culture: {info.Culture} ");

                if (!String.IsNullOrEmpty(AssistantSettings.VoiceName))
                {
                    if (info.Name == AssistantSettings.VoiceName)
                    {
                        synthesizer.SelectVoice(AssistantSettings.VoiceName);
                        voiceSelected = true;
                    }
                }
                if (!voiceSelected
                    && !String.IsNullOrEmpty(AssistantSettings.SpeakerCulture)
                    && info.Culture.Name.StartsWith(AssistantSettings.SpeakerCulture))
                {
                    synthesizer.SelectVoice(info.Name);
                }
            }

            var builder = new PromptBuilder();
            Console.WriteLine($"Selected voice: {synthesizer.Voice.Name}");

            return AudioOutSingleton.GetInstance(AssistantSettings.SpeakerCulture, synthesizer, builder, waveOut, AssistantSettings.AudioOutSampleRate);
        }

        private static WaveInEvent InitAudioInput()
        {
            var recordDeviceNumber = WaveIn.DeviceCount;
            var recordInputs = new Dictionary<Int32, String>(recordDeviceNumber + 1);
            Console.WriteLine("\r\nAvailable input devices:");
            var selectedDevice = -1;

            for (var n = -1; n < recordDeviceNumber; n++)
            {
                var caps = WaveIn.GetCapabilities(n);
                recordInputs.Add(n, caps.ProductName);
                Console.WriteLine($"- {n}: {caps.ProductName}");

                if (!String.IsNullOrEmpty(AssistantSettings.SelectedAudioInDevice) &&
                    caps.ProductName.StartsWith(AssistantSettings.SelectedAudioInDevice))
                {
                    selectedDevice = n;
                }
            }

            recordInputs.TryGetValue(selectedDevice, out var inDevice);

            var waveIn = new WaveInEvent
            {
                DeviceNumber = selectedDevice,
                WaveFormat = new WaveFormat(AssistantSettings.AudioInSampleRate, 1)
            };

            Console.WriteLine($"Selected input device: {inDevice}");
            Console.WriteLine($"Stream settings: {waveIn.WaveFormat}");

            return waveIn;
        }

        private static WaveOut InitAudioOutput()
        {
            var audioDeviceNumber = WaveOut.DeviceCount;
            var recordOutputs = new Dictionary<int, string>(audioDeviceNumber + 1);
            var selectedDevice = -1;
            Console.WriteLine("\r\nAvailable output devices:");

            for (var n = -1; n < audioDeviceNumber; n++)
            {
                var caps = WaveOut.GetCapabilities(n);
                recordOutputs.Add(n, caps.ProductName);
                Console.WriteLine($"- {n}: {caps.ProductName}");

                if (!string.IsNullOrEmpty(AssistantSettings.SelectedAudioOutDevice) &&
                    caps.ProductName.StartsWith(AssistantSettings.SelectedAudioOutDevice))
                {
                    selectedDevice = n;
                }
            }

            recordOutputs.TryGetValue(selectedDevice, out var outDevice);

            var waveOut = new WaveOut
            {
                DeviceNumber = selectedDevice
            };

            Console.WriteLine($"Selected output device: {outDevice}");

            return waveOut;
        }

        public static void ChangeRecognizeState()
        {
            RecognizeState = !RecognizeState;
        }

        public static void ChangeRecognizeState(Boolean value)
        {
            RecognizeState = value;
        }

        public static Boolean SetSynthesizerRate(Int16 rate)
        {
            SynthesizerRate = rate;

            return true;
        }

        public static Boolean SetDefoltInputDevice()
        {
            try
            {
                _waveIn = InitAudioInput();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean SetDefoltOutDevice()
        {
            try
            {
                _waveOut = InitAudioOutput();
            }
            catch
            {
                return false;
            }

            return true;
        }

        private static Boolean AddSpeechRecogniz()
        {
            try
            {
                _waveIn.DataAvailable += ProcessAudioInput;
                _waveIn.RecordingStopped += (s, a) => { _waveIn.Dispose(); };
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static void GiveSpeackText(String answer, TextBlock textBlock)
        {
            if (String.IsNullOrEmpty(answer) || 
                textBlock is null)
            {
                return;
            }

            var tmpTheard = new Thread(() =>
            {
                SpeackText(answer, textBlock);
            });

            tmpTheard.IsBackground = true;
            tmpTheard.Start();
        }

        public static void GiveSpeackText(String answer)
        {
            var textBlock = MainWindow.GetInstance().DuneAnswer;

            if (String.IsNullOrEmpty(answer) || textBlock is null)
            {
                return ;
            }

            var tmpTheard = new Thread(() =>
            {
                SpeackText(answer, textBlock);
            });

            tmpTheard.IsBackground = true;
            tmpTheard.Start();
        }

        private static void SpeackText(String text, TextBlock textBlock)
        {
            if (String.IsNullOrEmpty(text) || 
                textBlock is null)
            {
                return;
            }

            Invokes.EditTextBlock_Text(textBlock, text);

            if (RecognizeState)
            {
                Thread.Sleep(100);
                StopRecognize();
                SynthesizerSpeack(text);
                if (RecognizeState)
                {
                    StartRecognize();
                }
            }

            Thread.Sleep(100);
            Invokes.EditTextBlock_Text(textBlock, "...");
        }

        private static Boolean SynthesizerSpeack(String text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            try
            {
                _audioOut?.Speak(text, SynthesizerRate);
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean StopRecognize()
        {
            try
            {
                _waveIn.StopRecording();
            }
            catch
            {
                return false;
            }

            return true;
        }

        public static Boolean StartRecognize()
        {
            try
            {
                _waveIn.StartRecording();
            }
            catch
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}